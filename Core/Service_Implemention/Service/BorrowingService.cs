using AutoMapper;
using Domain_Layer.Contract.UnitOfWork;
using Domain_Layer.Models.Book_Models;
using Domain_Layer.Models.Borrow_Models;
using Domain_Layer.Models.Employee_Models;
using Domain_Layer.Models.Users_Models;
using Microsoft.EntityFrameworkCore;
using Service_Abstraction.Interfaces;
using Service_Implemention.Specification;
using Shared;
using Shared.DTO.Borrow;

namespace Service_Implemention.Service
{
    public class BorrowingService(IUnitOfWork _unitOfWork, IMapper _mapper) : IBorrowService
    {
        #region GetAllAsync
        public async Task<PaginatedResult<BorrowDTO>> GetAllAsync(BorrowQueryParamter borrowQuery)
        {
            var Specification = new BorrowSpecification(borrowQuery);
            var Borrow = await _unitOfWork.GetRepoartory<Borrow>().GetAllAsync(Specification);
            var BorrowDto = _mapper.Map<IEnumerable<Borrow>, IEnumerable<BorrowDTO>>(Borrow);

            #region Paggention
            var spec = new BorrowCountSpeciation(borrowQuery);
            var TotalCount = await _unitOfWork.GetRepoartory<Borrow>().CountAsync(spec);
            #endregion

            return new PaginatedResult<BorrowDTO>(TotalCount, Borrow.Count(), borrowQuery.PageIndex, BorrowDto);
        }
        #endregion

        #region GetByIdAsync

        public async Task<BorrowDTO?> GetByIdAsync(int userId, int bookId, DateTime dateBorrow)
        {
            var repo = _unitOfWork.GetRepoartory<Borrow>();

            var list = await repo.GetAllAsync(b =>
                b.UserId == userId &&
                b.BookId == bookId &&
                b.DateBorrow == dateBorrow);

            var entity = list.FirstOrDefault();
            return entity == null ? null : _mapper.Map<BorrowDTO>(entity);
        }


        #endregion

        #region Create

        public async Task<bool> CreateAsync(CreateOrUpdateBorrowDTO createBorrow)
        {
            try
            {
                // تحقّق أساسي من القيم
                if (createBorrow is null) return false;
                if (createBorrow.Amount <= 0)
                    throw new ArgumentException("الكمية المستعارة يجب أن تكون أكبر من صفر.");
                if (createBorrow.DueDate <= createBorrow.DateBorrow)
                    throw new ArgumentException("تاريخ الاستحقاق يجب أن يكون بعد تاريخ الاستعارة.");

                // ريبو
                var borrowRepo = _unitOfWork.GetRepoartory<Borrow>();
                var bookRepo = _unitOfWork.GetRepoartory<Book>();
                var userRepo = _unitOfWork.GetRepoartory<Users>();
                var empRepo = _unitOfWork.GetRepoartory<Employee>();

                // تحقّق من وجود الأطراف
                if (!await userRepo.AnyAsync(u => u.Id == createBorrow.UserId))
                    throw new ArgumentException("المستخدم غير موجود.");

                if (createBorrow.EmployeeId.HasValue &&
                    !await empRepo.AnyAsync(e => e.Id == createBorrow.EmployeeId.Value))
                    throw new ArgumentException("الموظف غير موجود.");

                var book = await bookRepo.GetByIdAsync(createBorrow.BookId);
                if (book is null)
                    throw new ArgumentException("الكتاب غير موجود.");

                // مخزون الكتاب
                if (book.Amount < createBorrow.Amount)
                    throw new InvalidOperationException("المخزون غير كافٍ لإتمام الاستعارة.");

                // خصم من المخزون
                book.Amount -= createBorrow.Amount;

                // إنشاء سجل Borrow عبر AutoMapper
                var borrow = _mapper.Map<Borrow>(createBorrow);

                await borrowRepo.AddAsync(borrow);

                // حفظ
                var ok = await _unitOfWork.SaveChangesAsync() > 0;
                return ok;
            }
            catch
            {
                // TODO: لو عندك ILogger، سجّل الاستثناء هنا
                return false;
            }
        }

        #endregion

        #region Update

        //public async Task<bool> UpdateAsync(int id, CreateOrUpdateBorrowDTO updateBorrow)
        //{
        //    try
        //    {
        //        if (updateBorrow is null) return false;

        //        // تحقق أساسي من القيم
        //        if (updateBorrow.Amount <= 0)
        //            throw new ArgumentException("الكمية المستعارة يجب أن تكون أكبر من صفر.");

        //        if (updateBorrow.DueDate <= updateBorrow.DateBorrow)
        //            throw new ArgumentException("تاريخ الاستحقاق يجب أن يكون بعد تاريخ الاستعارة.");

        //        // الريبو
        //        var borrowRepo = _unitOfWork.GetRepoartory<Borrow>();
        //        var bookRepo = _unitOfWork.GetRepoartory<Book>();
        //        var userRepo = _unitOfWork.GetRepoartory<Users>();
        //        var empRepo = _unitOfWork.GetRepoartory<Employee>();

        //        // جلب سجل الاستعارة (Tracked)
        //        var borrow = await borrowRepo.GetByIdAsync(id);
        //        if (borrow is null) return false;

        //        // تحقق من وجود الأطراف الجديدة
        //        if (!await userRepo.AnyAsync(u => u.Id == updateBorrow.UserId))
        //            throw new ArgumentException("المستخدم غير موجود.");

        //        if (updateBorrow.EmployeeId.HasValue &&
        //            !await empRepo.AnyAsync(e => e.Id == updateBorrow.EmployeeId.Value))
        //            throw new ArgumentException("الموظف غير موجود.");

        //        // جلب الكتب المعنيّة
        //        var oldBook = await bookRepo.GetByIdAsync(borrow.BookId);
        //        if (oldBook is null) throw new ArgumentException("الكتاب القديم غير موجود.");

        //        Book? newBook = oldBook;
        //        if (updateBorrow.BookId != borrow.BookId)
        //        {
        //            newBook = await bookRepo.GetByIdAsync(updateBorrow.BookId);
        //            if (newBook is null) throw new ArgumentException("الكتاب الجديد غير موجود.");
        //        }

        //        // حساب الفرق في الكمية
        //        int oldAmt = borrow.Amount;
        //        int newAmt = updateBorrow.Amount;

        //        if (updateBorrow.BookId != borrow.BookId)
        //        {
        //            // تغيّر الكتاب:
        //            // 1) إعادة مخزون الكتاب القديم بقيمة الاستعارة القديمة
        //            oldBook.Amount += oldAmt;

        //            // 2) خصم من مخزون الكتاب الجديد بقيمة الاستعارة الجديدة
        //            if (newBook!.Amount < newAmt)
        //                throw new InvalidOperationException("المخزون غير كافٍ في الكتاب الجديد.");

        //            newBook.Amount -= newAmt;

        //            // تعيين الكتاب الجديد في السجل
        //            borrow.BookId = updateBorrow.BookId;
        //        }
        //        else
        //        {
        //            // نفس الكتاب: عدّل المخزون بالفرق
        //            int delta = newAmt - oldAmt;
        //            if (delta > 0)
        //            {
        //                // زيادة الاستعارة → خصم من المخزون
        //                if (oldBook.Amount < delta)
        //                    throw new InvalidOperationException("المخزون غير كافٍ لزيادة الكمية المستعارة.");

        //                oldBook.Amount -= delta;
        //            }
        //            else if (delta < 0)
        //            {
        //                // تقليل الاستعارة → إعادة للمخزون
        //                oldBook.Amount += (-delta);
        //            }
        //            // لو delta == 0 → لا تغيير في المخزون
        //        }

        //        // تحديث باقي الحقول
        //        borrow.UserId = updateBorrow.UserId;
        //        borrow.EmployeeId = updateBorrow.EmployeeId; // قد تكون null
        //        borrow.DateBorrow = updateBorrow.DateBorrow;
        //        borrow.DueDate = updateBorrow.DueDate;
        //        borrow.Amount = newAmt;

        //        // مابنج اختياري لو حابب تستخدمه للحقول فقط (بدون النفيجيشن):
        //        // _mapper.Map(updateBorrow, borrow);

        //        borrowRepo.Update(borrow);

        //        return await _unitOfWork.SaveChangesAsync() > 0;
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        // لو عايز: سجّل الاستثناء وتعامل مع التزامن التفاؤلي لو فعّلت RowVersion على Book/Borrow
        //        return false;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        //public async Task<bool> UpdateByKeyAsync(int userId, int bookId, DateTime dateBorrow, CreateOrUpdateBorrowDTO dto)
        //{
        //    try
        //    {
        //        if (dto is null) return false;
        //        if (dto.Amount <= 0)
        //            throw new ArgumentException("الكمية المستعارة يجب أن تكون أكبر من صفر.");
        //        if (dto.DueDate <= dto.DateBorrow)
        //            throw new ArgumentException("تاريخ الاستحقاق يجب أن يكون بعد تاريخ الاستعارة.");

        //        var borrowRepo = _unitOfWork.GetRepoartory<Borrow>();
        //        var bookRepo = _unitOfWork.GetRepoartory<Book>();
        //        var userRepo = _unitOfWork.GetRepoartory<Users>();
        //        var empRepo = _unitOfWork.GetRepoartory<Employee>();

        //        // 1) هات السجل بالمفتاح المركّب (بدون Query())
        //        var list = await borrowRepo.GetAllAsync(b =>
        //            b.UserId == userId &&
        //            b.BookId == bookId &&
        //            b.DateBorrow == dateBorrow);

        //        var borrow = list.FirstOrDefault();
        //        if (borrow is null) return false;

        //        // 2) تحقّق الأطراف (على القيم الجديدة)
        //        if (!await userRepo.AnyAsync(u => u.Id == dto.UserId))
        //            throw new ArgumentException("المستخدم غير موجود.");
        //        if (dto.EmployeeId.HasValue &&
        //            !await empRepo.AnyAsync(e => e.Id == dto.EmployeeId.Value))
        //            throw new ArgumentException("الموظف غير موجود.");

        //        // 3) ممنوع تغيير قيم الـ PK في هذا السيناريو
        //        if (dto.UserId != userId || dto.BookId != bookId || dto.DateBorrow != dateBorrow)
        //            throw new InvalidOperationException("لا يمكن تغيير UserId/BookId/DateBorrow في هذا التحديث. استخدم مسار نقل (Delete + Insert).");

        //        // 4) عدّل المخزون لو تغيّر Amount
        //        var book = await bookRepo.GetByIdAsync(borrow.BookId);
        //        if (book is null) throw new ArgumentException("الكتاب غير موجود.");

        //        int oldAmt = borrow.Amount;
        //        int newAmt = dto.Amount;
        //        int delta = newAmt - oldAmt;

        //        if (delta > 0)
        //        {
        //            // زيادة الاستعارة => خصم من المخزون
        //            if (book.Amount < delta)
        //                throw new InvalidOperationException("المخزون غير كافٍ لزيادة الكمية.");
        //            book.Amount -= delta;
        //        }
        //        else if (delta < 0)
        //        {
        //            // تقليل الاستعارة => إعادة للمخزون
        //            book.Amount += (-delta);
        //        }

        //        // 5) حدّث بقية الحقول غير المفتاحية
        //        borrow.EmployeeId = dto.EmployeeId;
        //        borrow.DueDate = dto.DueDate;
        //        borrow.Amount = newAmt;
        //        // ملاحظة: DateBorrow جزء من الـ PK في هذا السيناريو فلا نغيّره
        //        // أيضاً UserId/BookId لا نغيرهم هنا

        //        borrowRepo.Update(borrow);
        //        return await _unitOfWork.SaveChangesAsync() > 0;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        public async Task<bool> UpdateByKeyAsync(int oldUserId, int oldBookId, DateTime oldDateBorrow, CreateOrUpdateBorrowDTO newBorrow)
        {
            try
            {
                if (newBorrow is null) return false;
                if (newBorrow.Amount <= 0)
                    throw new ArgumentException("الكمية المستعارة يجب أن تكون أكبر من صفر.");
                if (newBorrow.DueDate <= newBorrow.DateBorrow)
                    throw new ArgumentException("تاريخ الاستحقاق يجب أن يكون بعد تاريخ الاستعارة.");

                var borrowRepo = _unitOfWork.GetRepoartory<Borrow>();
                var bookRepo = _unitOfWork.GetRepoartory<Book>();
                var userRepo = _unitOfWork.GetRepoartory<Users>();
                var empRepo = _unitOfWork.GetRepoartory<Employee>();

                // هات القديم
                var oldList = await borrowRepo.GetAllAsync(b =>
                    b.UserId == oldUserId &&
                    b.BookId == oldBookId &&
                    b.DateBorrow == oldDateBorrow);
                var oldBorrow = oldList.FirstOrDefault();
                if (oldBorrow is null) return false;

                // تحقّق الأطراف الجديدة
                if (!await userRepo.AnyAsync(u => u.Id == newBorrow.UserId))
                    throw new ArgumentException("المستخدم الجديد غير موجود.");
                if (newBorrow.EmployeeId.HasValue &&
                    !await empRepo.AnyAsync(e => e.Id == newBorrow.EmployeeId.Value))
                    throw new ArgumentException("الموظف الجديد غير موجود.");

                // رجّع مخزون الكتاب القديم بالكامل
                var oldBook = await bookRepo.GetByIdAsync(oldBorrow.BookId);
                if (oldBook is null) throw new ArgumentException("الكتاب القديم غير موجود.");
                oldBook.Amount += oldBorrow.Amount;

                // جهّز السجل الجديد
                var newBook = await bookRepo.GetByIdAsync(newBorrow.BookId);
                if (newBook is null) throw new ArgumentException("الكتاب الجديد غير موجود.");
                if (newBook.Amount < newBorrow.Amount)
                    throw new InvalidOperationException("المخزون غير كافٍ في الكتاب الجديد.");

                newBook.Amount -= newBorrow.Amount;

                // احذف القديم
                borrowRepo.Remove(oldBorrow);

                // أضف الجديد
                var newEntity = _mapper.Map<Borrow>(newBorrow);
                await borrowRepo.AddAsync(newEntity);

                return await _unitOfWork.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Delete
        //public async Task<bool> DeleteAsync(int id)
        //{
        //    try
        //    {
        //        var borrowRepo = _unitOfWork.GetRepoartory<Borrow>();
        //        var bookRepo = _unitOfWork.GetRepoartory<Book>();

        //        // جلب سجل الاستعارة (Tracked)
        //        var borrow = await borrowRepo.GetByIdAsync(id);
        //        if (borrow is null) return false;

        //        // جلب الكتاب المرتبط لتحديث المخزون
        //        var book = await bookRepo.GetByIdAsync(borrow.BookId);
        //        if (book is null) throw new ArgumentException("Book not found.");

        //        // إعادة المخزون بالكمية المستعارة
        //        book.Amount += borrow.Amount;

        //        // حذف السجل
        //        borrowRepo.Remove(borrow);

        //        return await _unitOfWork.SaveChangesAsync() > 0;
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        // لو فيه قيود مانعة (FK/سياسات)، سجّل السبب (اختياري)
        //        Console.WriteLine(ex.Message);
        //        return false;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        public async Task<bool> DeleteByKeyAsync(int userId, int bookId, DateTime dateBorrow)
        {
            try
            {
                var borrowRepo = _unitOfWork.GetRepoartory<Borrow>();
                var bookRepo = _unitOfWork.GetRepoartory<Book>();

                // 1) هات السجل بالمفتاح المركّب (بدون Query)
                var list = await borrowRepo.GetAllAsync(b =>
                    b.UserId == userId &&
                    b.BookId == bookId &&
                    b.DateBorrow == dateBorrow);

                var borrow = list.FirstOrDefault();
                if (borrow is null) return false; // NotFound

                // 2) هات الكتاب وارجع المخزون
                var book = await bookRepo.GetByIdAsync(borrow.BookId);
                if (book is null) throw new ArgumentException("Book not found.");

                // إعادة المخزون بالكمية المستعارة
                book.Amount += borrow.Amount;

                // 3) احذف السجل
                borrowRepo.Remove(borrow);

                // 4) حفظ
                return await _unitOfWork.SaveChangesAsync() > 0;
            }
            catch (DbUpdateException ex)
            {
                //            // لو فيه قيود FK تمنع الحذف (سياساتك)، سجّل السبب
                Console.WriteLine(ex.Message);
                return false;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}

