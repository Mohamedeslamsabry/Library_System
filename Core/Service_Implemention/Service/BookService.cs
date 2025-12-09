using AutoMapper;
using Domain_Layer.Contract.UnitOfWork;
using Domain_Layer.Models.Book_Authors_Models;
using Domain_Layer.Models.Book_Models;
using Domain_Layer.Models.Categories_Models;
using Domain_Layer.Models.Puplishers_Models;
using Domain_Layer.Models.Shelf_Models;
using Microsoft.EntityFrameworkCore;
using Service_Abstraction.Interfaces;
using Service_Implemention.Specification;
using Shared;
using Shared.DTO.Book;

namespace Service_Implemention.Service
{
    public class BookService(IUnitOfWork _unitOfWork, IMapper _mapper) : IBookService
    {
        #region GetAllAsync
        public async Task<PaginatedResult<BookDTO>> GetAllAsync(BookQueryPartmer bookQuery)
        {
            var Specification = new BookSpecifaction(bookQuery);
            var Books = await _unitOfWork.GetRepoartory<Book>().GetAllAsync(Specification);
            var BookDto = _mapper.Map<IEnumerable<Book>, IEnumerable<BookDTO>>(Books);

            #region Paggention
            var spec = new BookCountSpecification(bookQuery);
            var TotalCount = await _unitOfWork.GetRepoartory<Book>().CountAsync(spec);
            #endregion

            return new PaginatedResult<BookDTO>(TotalCount, Books.Count(), bookQuery.PageIndex, BookDto);
        }
        #endregion

        #region GetByIdAsync
        public async Task<BookDTO?> GetByIdAsync(int id)
        {
            var Book = await _unitOfWork.GetRepoartory<Book>().GetByIdAsync(id);
            return Book == null ? null : _mapper.Map<BookDTO>(Book);
        }
        #endregion

        #region CreateAsync
        public async Task<bool> CreateAsync(CreateOrUpdateBookDto createBook)
        {
            try
            {
                // ✅ بلاش .Result — استخدم await
                bool nameExist = await _unitOfWork
                    .GetRepoartory<Book>()
                    .AnyAsync(x => x.Name == createBook.Name); // أو x.TiTle == createBook.Title

                if (nameExist) return false;

                // تحقق من العلاقات (لاحظ استخدام .Value)
                if (createBook.PublisherId.HasValue &&
                    !await _unitOfWork.GetRepoartory<Puplishers>().AnyAsync(f => f.Id == createBook.PublisherId.Value))
                    throw new ArgumentException("Publisher does not exist.");

                if (createBook.CategoryId.HasValue &&
                    !await _unitOfWork.GetRepoartory<Categories>().AnyAsync(f => f.Id == createBook.CategoryId.Value))
                    throw new ArgumentException("Categories does not exist.");

                if (createBook.ShelfId.HasValue &&
                    !await _unitOfWork.GetRepoartory<Shelf>().AnyAsync(f => f.Id == createBook.ShelfId.Value))
                    throw new ArgumentException("Shelf does not exist.");

                // 1) خَلق الكتاب فقط (من غير أي روابط مؤلفين)
                var book = _mapper.Map<CreateOrUpdateBookDto, Book>(createBook);

                // تأكد إن المجموعة فاضية (لو المابنج كان بيحط حاجة)
                book.Book_Authors = new HashSet<Book_Authors>();

                await _unitOfWork.GetRepoartory<Book>().AddAsync(book);
                var saved = await _unitOfWork.SaveChangesAsync() > 0;
                if (!saved) return false; // لو فشل الحفظ، وقف هنا

                // 2) أضف روابط المؤلفين بعد ما book.Id بقى معروف
                var linksRepo = _unitOfWork.GetRepoartory<Book_Authors>();

                var authorIds = (createBook.AuthorIds ?? new List<int>())
                                .Where(id => id > 0)
                                .Distinct()
                                .ToList();

                foreach (var authorId in authorIds)
                {
                    // مهم: تأكد مفيش رابط بنفس المفتاح متتبَّع أو موجود
                    var exists = await linksRepo.AnyAsync(l => l.BookId == book.Id && l.AuthorId == authorId);
                    if (exists) continue;

                    await linksRepo.AddAsync(new Book_Authors
                    {
                        BookId = book.Id,
                        AuthorId = authorId
                    });
                }

                return await _unitOfWork.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region UpdateAsync
        public async Task<bool> UpdateAsync(int id, CreateOrUpdateBookDto updateBook)
        {
            try
            {
                var bookRepo = _unitOfWork.GetRepoartory<Book>();

                // تحقّق من الاسم (غَيّر Name إلى TiTle لو الكيان عندك مختلف)
                bool nameExist = await bookRepo.AnyAsync(x => x.Name == updateBook.Name && x.Id != id);
                if (nameExist)
                    return false;

                // جلب الكتاب (Tracked للـ Lazy Loading)
                var book = await bookRepo.GetByIdAsync(id);
                if (book is null)
                    return false;

                // التحقق من المفاتيح المرتبطة (استخدم .Value مع nullable)
                if (updateBook.PublisherId.HasValue &&
                    !await _unitOfWork.GetRepoartory<Puplishers>()
                        .AnyAsync(f => f.Id == updateBook.PublisherId.Value))
                    throw new ArgumentException("Publisher does not exist.");

                if (updateBook.CategoryId.HasValue &&
                    !await _unitOfWork.GetRepoartory<Categories>()
                        .AnyAsync(f => f.Id == updateBook.CategoryId.Value))
                    throw new ArgumentException("Categories does not exist.");

                if (updateBook.ShelfId.HasValue &&
                    !await _unitOfWork.GetRepoartory<Shelf>()
                        .AnyAsync(f => f.Id == updateBook.ShelfId.Value))
                    throw new ArgumentException("Shelf does not exist.");

                // مابنج للخصائص البسيطة فقط (Book_Authors متجاهلة في الـ Profile)
                _mapper.Map(updateBook, book);

                // مزامنة المؤلفين بشكل نظيف (إضافة/حذف بدون تكرار)
                await SyncAuthorsAsync(book, updateBook.AuthorIds);

                bookRepo.Update(book);
                return await _unitOfWork.SaveChangesAsync() > 0;
            }
            catch (DbUpdateConcurrencyException)
            {
                // ممكن تسجّل الاستثناء وتتعامل مع الـ concurrency لو بتستخدمه
                return false;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region DeleteAsync
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var bookRepo = _unitOfWork.GetRepoartory<Book>();
                var linkRepo = _unitOfWork.GetRepoartory<Book_Authors>();

                var book = await bookRepo.GetByIdAsync(id);
                if (book is null) return false;

                // لو عايز تمنع الحذف لو فيه Borrow مرتبط:
                if (book.Borrow != null) throw new InvalidOperationException("Cannot delete a borrowed book.");

                // حمّل مجموعة الروابط (لو Lazy Loading عبر proxies، لمس المجموعة بيكفي)
                book.Book_Authors ??= new HashSet<Book_Authors>();
                var _ = book.Book_Authors.Count; // يجبر Lazy Loading على التحميل

                // احذف الروابط أولًا
                if (book.Book_Authors.Any())
                {
                    // الأفضل تستخدم Remove على الـ repo للروابط
                    foreach (var link in book.Book_Authors.ToList())
                        linkRepo.Remove(link);
                }

                // بعد تنظيف الروابط، احذف الكتاب
                bookRepo.Remove(book);

                return await _unitOfWork.SaveChangesAsync() > 0;
            }
            catch (DbUpdateException ex)
            {
                // في حالة وجود علاقات أخرى تمنع الحذف (مثلاً Borrow بـ Restrict)
                // TODO: log ex
                Console.WriteLine(ex.Message);
                return false;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Helper
        private async Task SyncAuthorsAsync(Book book, IEnumerable<int>? authorIds)
        {
            var desired = (authorIds ?? Enumerable.Empty<int>())
                .Where(id => id > 0)
                .Distinct()
                .ToHashSet();

            book.Book_Authors ??= new HashSet<Book_Authors>();

            // إجبار Lazy Loading يحمل المجموعة لو بتستخدم proxies
            var _ = book.Book_Authors.Count;

            var linksRepo = _unitOfWork.GetRepoartory<Book_Authors>();

            // احذف الروابط الزائدة
            var toRemove = book.Book_Authors.Where(x => !desired.Contains(x.AuthorId)).ToList();
            foreach (var link in toRemove)
                linksRepo.Remove(link);

            // أضف الروابط الناقصة
            var current = book.Book_Authors.Select(x => x.AuthorId).ToHashSet();
            var toAdd = desired.Where(id => !current.Contains(id));
            foreach (var id in toAdd)
            {
                // أمان إضافي: لو فيه رابط بنفس المفتاح متتبّع أو موجود، تجاهله
                if (await linksRepo.AnyAsync(l => l.BookId == book.Id && l.AuthorId == id))
                    continue;

                await linksRepo.AddAsync(new Book_Authors
                {
                    BookId = book.Id,   // معروف في التحديث
                    AuthorId = id
                });
            }

        }

        #endregion
    }
}