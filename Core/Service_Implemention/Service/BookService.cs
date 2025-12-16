using AutoMapper;
using Domain_Layer.Contract.UnitOfWork;
using Domain_Layer.Exceptions;
using Domain_Layer.Models.Book_Authors_Models;
using Domain_Layer.Models.Book_Models;
using Domain_Layer.Models.Borrow_Models;
using Domain_Layer.Models.Categories_Models;
using Domain_Layer.Models.Puplishers_Models;
using Domain_Layer.Models.Shelf_Models;
using Microsoft.EntityFrameworkCore;
using Service_Abstraction.Interfaces;
using Service_Implemention.Specification;
using Shared;
using Shared.DTO.Book;
using Shared.Error;

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
            return Book == null ? throw new BookNotFoundException(id) : _mapper.Map<BookDTO>(Book);
        }
        #endregion

        #region CreateAsync
        public async Task<Result<int>> CreateAsync(CreateOrUpdateBookDto createBook)
        {
            if (createBook is null)
                return Result<int>.Fail("Data not found", ErrorCodes.ValidationNull);

            if (string.IsNullOrWhiteSpace(createBook.Name))
                return Result<int>.Fail("Book name is required", ErrorCodes.NameRequired);

            try
            {
                var bookRepo = _unitOfWork.GetRepoartory<Book>();
                var linksRepo = _unitOfWork.GetRepoartory<Book_Authors>();

                var normalizedName = createBook.Name.Trim().ToLower();
                var nameExists = await bookRepo.AnyAsync(x => x.Name.ToLower() == normalizedName);
                if (nameExists)
                    return Result<int>.Fail("The name of the book already exists", ErrorCodes.BookDuplicateName);

                if (createBook.PublisherId.HasValue &&
                    !await _unitOfWork.GetRepoartory<Puplishers>().AnyAsync(f => f.Id == createBook.PublisherId.Value))
                    return Result<int>.Fail("Publisher not found", ErrorCodes.PublisherNotFound);

                if (createBook.CategoryId.HasValue &&
                    !await _unitOfWork.GetRepoartory<Categories>().AnyAsync(f => f.Id == createBook.CategoryId.Value))
                    return Result<int>.Fail("Categories not found", ErrorCodes.CategoryNotFound);

                if (createBook.ShelfId.HasValue &&
                    !await _unitOfWork.GetRepoartory<Shelf>().AnyAsync(f => f.Id == createBook.ShelfId.Value))
                    return Result<int>.Fail("Shelf not found", ErrorCodes.ShelfNotFound);

                var book = _mapper.Map<CreateOrUpdateBookDto, Book>(createBook);
                book.Book_Authors = new HashSet<Book_Authors>();

                await bookRepo.AddAsync(book);
                var saved = await _unitOfWork.SaveChangesAsync() > 0;
                if (!saved)
                    return Result<int>.Fail("Failed to save data", ErrorCodes.DbSaveFailed);

                var authorIds = (createBook.AuthorIds ?? new List<int>())
                                .Where(id => id > 0)
                                .Distinct()
                                .ToList();

                foreach (var authorId in authorIds)
                {
                    var exists = await linksRepo.AnyAsync(l => l.BookId == book.Id && l.AuthorId == authorId);
                    if (exists) continue;

                    await linksRepo.AddAsync(new Book_Authors
                    {
                        BookId = book.Id,
                        AuthorId = authorId
                    });
                }

                var linksSaved = await _unitOfWork.SaveChangesAsync() > 0;

                return Result<int>.Ok(book.Id, "The book was created successfully");
            }
            catch (OperationCanceledException)
            {
                return Result<int>.Fail("The operation has been cancelled", ErrorCodes.Canceled);
            }
            catch (AutoMapperMappingException)
            {
                return Result<int>.Fail("Data conversion failed (Mapping)", ErrorCodes.MappingError);
            }
            catch (DbUpdateException)
            {
                return Result<int>.Fail("A database error occurred during saving.", ErrorCodes.DbUpdateError);
            }
            catch (Exception)
            {
                return Result<int>.Fail("An unexpected error occurred", ErrorCodes.Unexpected);
            }
        }
        #endregion

        #region UpdateAsync
        //public async Task<bool> UpdateAsync(int id, CreateOrUpdateBookDto updateBook)
        //{
        //    try
        //    {
        //        var bookRepo = _unitOfWork.GetRepoartory<Book>();

        //        // تحقّق من الاسم (غَيّر Name إلى TiTle لو الكيان عندك مختلف)
        //        bool nameExist = await bookRepo.AnyAsync(x => x.Name == updateBook.Name && x.Id != id);
        //        if (nameExist)
        //            return false;

        //        // جلب الكتاب (Tracked للـ Lazy Loading)
        //        var book = await bookRepo.GetByIdAsync(id);
        //        if (book is null)
        //            return false;

        //        // التحقق من المفاتيح المرتبطة (استخدم .Value مع nullable)
        //        if (updateBook.PublisherId.HasValue &&
        //            !await _unitOfWork.GetRepoartory<Puplishers>()
        //                .AnyAsync(f => f.Id == updateBook.PublisherId.Value))
        //            throw new ArgumentException("Publisher does not exist.");

        //        if (updateBook.CategoryId.HasValue &&
        //            !await _unitOfWork.GetRepoartory<Categories>()
        //                .AnyAsync(f => f.Id == updateBook.CategoryId.Value))
        //            throw new ArgumentException("Categories does not exist.");

        //        if (updateBook.ShelfId.HasValue &&
        //            !await _unitOfWork.GetRepoartory<Shelf>()
        //                .AnyAsync(f => f.Id == updateBook.ShelfId.Value))
        //            throw new ArgumentException("Shelf does not exist.");

        //        // مابنج للخصائص البسيطة فقط (Book_Authors متجاهلة في الـ Profile)
        //        _mapper.Map(updateBook, book);

        //        // مزامنة المؤلفين بشكل نظيف (إضافة/حذف بدون تكرار)
        //        await SyncAuthorsAsync(book, updateBook.AuthorIds);

        //        bookRepo.Update(book);
        //        return await _unitOfWork.SaveChangesAsync() > 0;
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        // ممكن تسجّل الاستثناء وتتعامل مع الـ concurrency لو بتستخدمه
        //        return false;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        public async Task<Result<int>> UpdateAsync(int id, CreateOrUpdateBookDto updateBook)
        {
            if (updateBook is null)
                return Result<int>.Fail("Data not found", ErrorCodes.ValidationNull);

            if (string.IsNullOrWhiteSpace(updateBook.Name))
                return Result<int>.Fail("Book name is required", ErrorCodes.NameRequired);

            try
            {
                var bookRepo = _unitOfWork.GetRepoartory<Book>();

                var normalizedName = updateBook.Name.Trim().ToLower();
                var nameExists = await bookRepo.AnyAsync(x => x.Id != id && x.Name.ToLower() == normalizedName);
                if (nameExists)
                    return Result<int>.Fail("The name of the book already exists", ErrorCodes.BookDuplicateName);

                var book = await bookRepo.GetByIdAsync(id);
                if (book is null)
                    return Result<int>.Fail("Book not found", ErrorCodes.BookNotFound);

                if (updateBook.PublisherId.HasValue &&
                    !await _unitOfWork.GetRepoartory<Puplishers>().AnyAsync(f => f.Id == updateBook.PublisherId.Value))
                    return Result<int>.Fail("Publisher not found", ErrorCodes.PublisherNotFound);

                if (updateBook.CategoryId.HasValue &&
                    !await _unitOfWork.GetRepoartory<Categories>().AnyAsync(f => f.Id == updateBook.CategoryId.Value))
                    return Result<int>.Fail("Classification not found", ErrorCodes.CategoryNotFound);

                if (updateBook.ShelfId.HasValue &&
                    !await _unitOfWork.GetRepoartory<Shelf>().AnyAsync(f => f.Id == updateBook.ShelfId.Value))
                    return Result<int>.Fail("Shelf not found", ErrorCodes.ShelfNotFound);

                _mapper.Map(updateBook, book);

                await SyncAuthorsAsync(book, updateBook.AuthorIds);

                bookRepo.Update(book);
                var saved = await _unitOfWork.SaveChangesAsync() > 0;
                if (!saved)
                    return Result<int>.Fail("Failed to save data", ErrorCodes.DbSaveFailed);

                return Result<int>.Ok(book.Id, "The book has been successfully updated");
            }
            catch (OperationCanceledException)
            {
                return Result<int>.Fail("The operation has been cancelled", ErrorCodes.Canceled);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Result<int>.Fail("Update conflict(Concurrency)", ErrorCodes.ConcurrencyError);
            }
            catch (AutoMapperMappingException)
            {
                return Result<int>.Fail("Data conversion failed (Mapping)", ErrorCodes.MappingError);
            }
            catch (DbUpdateException)
            {
                return Result<int>.Fail("A database error occurred during saving.", ErrorCodes.DbUpdateError);
            }
            catch (Exception)
            {
                return Result<int>.Fail("An unexpected error occurred", ErrorCodes.Unexpected);
            }
        }
        #endregion

        #region DeleteAsync

        //public async Task<bool> DeleteAsync(int id)
        //{
        //    try
        //    {
        //        var bookRepo = _unitOfWork.GetRepoartory<Book>();
        //        var book = await bookRepo.GetByIdAsync(id);
        //        if (book is null) return false;

        //        // اجبار Lazy Loading لتحميل المجموعات
        //        book.Borrows ??= new HashSet<Borrow>();
        //        var _borrowsCount = book.Borrows.Count;

        //        if (_borrowsCount > 0)
        //            throw new InvalidOperationException("Cannot delete a book that has borrow records.");

        //        // نفس الشيء لعلاقة Book_Authors إن لم يكن لديك Cascade
        //        book.Book_Authors ??= new HashSet<Book_Authors>();
        //        var _authorsCount = book.Book_Authors.Count;
        //        if (_authorsCount > 0)
        //        {
        //            var linkRepo = _unitOfWork.GetRepoartory<Book_Authors>();
        //            foreach (var link in book.Book_Authors.ToList())
        //                linkRepo.Remove(link);
        //        }

        //        bookRepo.Remove(book);
        //        return await _unitOfWork.SaveChangesAsync() > 0;
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return false;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        public async Task<Result<int>> DeleteAsync(int id)
        {
            try
            {
                var bookRepo = _unitOfWork.GetRepoartory<Book>();
                var book = await bookRepo.GetByIdAsync(id);

                if (book is null)
                    return Result<int>.Fail("The book does not exist", ErrorCodes.BookNotFound);

                book.Borrows ??= new HashSet<Borrow>();
                var borrowsCount = book.Borrows.Count;

                if (borrowsCount > 0)
                    return Result<int>.Fail("A book with loan records cannot be deleted", ErrorCodes.BorrowExists, book.Id);

                book.Book_Authors ??= new HashSet<Book_Authors>();
                if (book.Book_Authors.Count > 0)
                {
                    var linkRepo = _unitOfWork.GetRepoartory<Book_Authors>();
                    foreach (var link in book.Book_Authors.ToList())
                        linkRepo.Remove(link);
                }

                bookRepo.Remove(book);

                var saved = await _unitOfWork.SaveChangesAsync() > 0;
                if (!saved)
                    return Result<int>.Fail("Failed to save data", ErrorCodes.DbSaveFailed, book.Id);

                return Result<int>.Ok(book.Id, "The book has been successfully deleted");
            }
            catch (OperationCanceledException)
            {
                return Result<int>.Fail("The operation has been cancelled", ErrorCodes.Canceled);
            }
            catch (DbUpdateException)
            {
                return Result<int>.Fail("The book could not be deleted due to database limitations.", ErrorCodes.DbUpdateError);
            }
            catch (Exception)
            {
                return Result<int>.Fail("An unexpected error occurred", ErrorCodes.Unexpected);
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