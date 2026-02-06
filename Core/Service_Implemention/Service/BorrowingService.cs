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
using Shared.Error;

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

        //public async Task<bool> CreateAsync(CreateOrUpdateBorrowDTO createBorrow)
        //{
        //    try
        //    {
        //        // تحقّق أساسي من القيم
        //        if (createBorrow is null) return false;
        //        if (createBorrow.Amount <= 0)
        //            throw new ArgumentException("الكمية المستعارة يجب أن تكون أكبر من صفر.");
        //        if (createBorrow.DueDate <= createBorrow.DateBorrow)
        //            throw new ArgumentException("تاريخ الاستحقاق يجب أن يكون بعد تاريخ الاستعارة.");

        //        // ريبو
        //        var borrowRepo = _unitOfWork.GetRepoartory<Borrow>();
        //        var bookRepo = _unitOfWork.GetRepoartory<Book>();
        //        var userRepo = _unitOfWork.GetRepoartory<Users>();
        //        var empRepo = _unitOfWork.GetRepoartory<Employee>();

        //        // تحقّق من وجود الأطراف
        //        if (!await userRepo.AnyAsync(u => u.Id == createBorrow.UserId))
        //            throw new ArgumentException("المستخدم غير موجود.");

        //        if (createBorrow.EmployeeId.HasValue &&
        //            !await empRepo.AnyAsync(e => e.Id == createBorrow.EmployeeId.Value))
        //            throw new ArgumentException("الموظف غير موجود.");

        //        var book = await bookRepo.GetByIdAsync(createBorrow.BookId);
        //        if (book is null)
        //            throw new ArgumentException("الكتاب غير موجود.");

        //        // مخزون الكتاب
        //        if (book.Amount < createBorrow.Amount)
        //            throw new InvalidOperationException("المخزون غير كافٍ لإتمام الاستعارة.");

        //        // خصم من المخزون
        //        book.Amount -= createBorrow.Amount;

        //        // إنشاء سجل Borrow عبر AutoMapper
        //        var borrow = _mapper.Map<Borrow>(createBorrow);

        //        await borrowRepo.AddAsync(borrow);

        //        // حفظ
        //        var ok = await _unitOfWork.SaveChangesAsync() > 0;
        //        return ok;
        //    }
        //    catch
        //    {
        //        // TODO: لو عندك ILogger، سجّل الاستثناء هنا
        //        return false;
        //    }
        //}

        public async Task<Result<int>> CreateAsync(CreateOrUpdateBorrowDTO createBorrow)
        {
            // Basic request validation
            if (createBorrow is null)
                return Result<int>.Fail("Request body is missing.", ErrorCodes.ValidationNull);

            if (createBorrow.Amount <= 0)
                return Result<int>.Fail("Borrow amount must be greater than zero.", ErrorCodes.AmountInvalid);

            if (createBorrow.DueDate <= createBorrow.DateBorrow)
                return Result<int>.Fail("Due date must be after borrow date.", ErrorCodes.DueDateInvalid);

            try
            {
                // Repositories
                var borrowRepo = _unitOfWork.GetRepoartory<Borrow>();
                var bookRepo = _unitOfWork.GetRepoartory<Book>();
                var userRepo = _unitOfWork.GetRepoartory<Users>();
                var empRepo = _unitOfWork.GetRepoartory<Employee>();

                // Validate parties
                var userExists = await userRepo.AnyAsync(u => u.Id == createBorrow.UserId);
                if (!userExists)
                    return Result<int>.Fail("User not found.", ErrorCodes.UserNotFound);

                if (createBorrow.EmployeeId.HasValue)
                {
                    var employeeExists = await empRepo.AnyAsync(e => e.Id == createBorrow.EmployeeId.Value);
                    if (!employeeExists)
                        return Result<int>.Fail("Employee not found.", ErrorCodes.EmployeeNotFound);
                }

                // Load book & validate stock
                var book = await bookRepo.GetByIdAsync(createBorrow.BookId);
                if (book is null)
                    return Result<int>.Fail("Book not found.", ErrorCodes.BookNotFound);

                if (book.Amount < createBorrow.Amount)
                    return Result<int>.Fail("Insufficient stock to complete the borrow.", ErrorCodes.StockInsufficient);

                // Deduct stock
                book.Amount -= createBorrow.Amount;

                // Create Borrow via AutoMapper
                var borrow = _mapper.Map<Borrow>(createBorrow);

                await borrowRepo.AddAsync(borrow);

                // Persist changes
                var saved = await _unitOfWork.SaveChangesAsync() > 0;
                if (!saved)
                    return Result<int>.Fail("Failed to save changes.", ErrorCodes.DbSaveFailed);

                return Result<int>.Ok(borrow.Id, "Borrow created successfully.");
            }
            catch (OperationCanceledException)
            {
                return Result<int>.Fail("Operation was canceled.", ErrorCodes.Canceled);
            }
            catch (AutoMapperMappingException)
            {
                return Result<int>.Fail("Data mapping failed.", ErrorCodes.MappingError);
            }
            catch (DbUpdateException)
            {
                return Result<int>.Fail("Database failed during create The error could be caused by the fact that this metadata already exists in the database.", ErrorCodes.DbUpdateError);
            }
            catch (Exception)
            {
                return Result<int>.Fail("An unexpected error occurred.", ErrorCodes.Unexpected);
            }
        }

        #endregion

        #region Update
        //public async Task<bool> UpdateByKeyAsync(int oldUserId, int oldBookId, DateTime oldDateBorrow, CreateOrUpdateBorrowDTO newBorrow)
        //{
        //    try
        //    {
        //        if (newBorrow is null) return false;
        //        if (newBorrow.Amount <= 0)
        //            throw new ArgumentException("الكمية المستعارة يجب أن تكون أكبر من صفر.");
        //        if (newBorrow.DueDate <= newBorrow.DateBorrow)
        //            throw new ArgumentException("تاريخ الاستحقاق يجب أن يكون بعد تاريخ الاستعارة.");

        //        var borrowRepo = _unitOfWork.GetRepoartory<Borrow>();
        //        var bookRepo = _unitOfWork.GetRepoartory<Book>();
        //        var userRepo = _unitOfWork.GetRepoartory<Users>();
        //        var empRepo = _unitOfWork.GetRepoartory<Employee>();

        //        // هات القديم
        //        var oldList = await borrowRepo.GetAllAsync(b =>
        //            b.UserId == oldUserId &&
        //            b.BookId == oldBookId &&
        //            b.DateBorrow == oldDateBorrow);
        //        var oldBorrow = oldList.FirstOrDefault();
        //        if (oldBorrow is null) return false;

        //        // تحقّق الأطراف الجديدة
        //        if (!await userRepo.AnyAsync(u => u.Id == newBorrow.UserId))
        //            throw new ArgumentException("المستخدم الجديد غير موجود.");
        //        if (newBorrow.EmployeeId.HasValue &&
        //            !await empRepo.AnyAsync(e => e.Id == newBorrow.EmployeeId.Value))
        //            throw new ArgumentException("الموظف الجديد غير موجود.");

        //        // رجّع مخزون الكتاب القديم بالكامل
        //        var oldBook = await bookRepo.GetByIdAsync(oldBorrow.BookId);
        //        if (oldBook is null) throw new ArgumentException("الكتاب القديم غير موجود.");
        //        oldBook.Amount += oldBorrow.Amount;

        //        // جهّز السجل الجديد
        //        var newBook = await bookRepo.GetByIdAsync(newBorrow.BookId);
        //        if (newBook is null) throw new ArgumentException("الكتاب الجديد غير موجود.");
        //        if (newBook.Amount < newBorrow.Amount)
        //            throw new InvalidOperationException("المخزون غير كافٍ في الكتاب الجديد.");

        //        newBook.Amount -= newBorrow.Amount;

        //        // احذف القديم
        //        borrowRepo.Remove(oldBorrow);

        //        // أضف الجديد
        //        var newEntity = _mapper.Map<Borrow>(newBorrow);
        //        await borrowRepo.AddAsync(newEntity);

        //        return await _unitOfWork.SaveChangesAsync() > 0;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        public async Task<Result<int>> UpdateByKeyAsync(
            int oldUserId,
            int oldBookId,
            DateTime oldDateBorrow,
            CreateOrUpdateBorrowDTO newBorrow)
        {
            // 0) Basic request validation
            if (newBorrow is null)
                return Result<int>.Fail("Request body is missing.", ErrorCodes.ValidationNull);

            if (newBorrow.Amount <= 0)
                return Result<int>.Fail("Borrow amount must be greater than zero.", ErrorCodes.AmountInvalid);

            if (newBorrow.DueDate <= newBorrow.DateBorrow)
                return Result<int>.Fail("Due date must be after borrow date.", ErrorCodes.DueDateInvalid);

            try
            {
                var borrowRepo = _unitOfWork.GetRepoartory<Borrow>();
                var bookRepo = _unitOfWork.GetRepoartory<Book>();
                var userRepo = _unitOfWork.GetRepoartory<Users>();
                var empRepo = _unitOfWork.GetRepoartory<Employee>();

                // 1) Fetch old borrow (by composite key)
                var oldList = await borrowRepo.GetAllAsync(b =>
                    b.UserId == oldUserId &&
                    b.BookId == oldBookId &&
                    b.DateBorrow == oldDateBorrow);

                var oldBorrow = oldList.FirstOrDefault();
                if (oldBorrow is null)
                    return Result<int>.Fail("Original borrow record not found.", ErrorCodes.OldBorrowNotFound);

                // 2) Validate new parties
                var userExists = await userRepo.AnyAsync(u => u.Id == newBorrow.UserId);
                if (!userExists)
                    return Result<int>.Fail("User not found.", ErrorCodes.UserNotFound);

                if (newBorrow.EmployeeId.HasValue)
                {
                    var employeeExists = await empRepo.AnyAsync(e => e.Id == newBorrow.EmployeeId.Value);
                    if (!employeeExists)
                        return Result<int>.Fail("Employee not found.", ErrorCodes.EmployeeNotFound);
                }

                // 3) Check for duplicate on *new* composite key before changing anything
                var duplicateExists = await borrowRepo.AnyAsync(b =>
                    b.UserId == newBorrow.UserId &&
                    b.BookId == newBorrow.BookId &&
                    b.DateBorrow == newBorrow.DateBorrow);
                if (duplicateExists)
                    return Result<int>.Fail("A borrow record already exists for the given user, book, and date.", ErrorCodes.BorrowDuplicate);

                // 4) Load books (old & new)
                var oldBook = await bookRepo.GetByIdAsync(oldBorrow.BookId);
                if (oldBook is null)
                    return Result<int>.Fail("Original book not found.", ErrorCodes.BookNotFound);

                var newBook = await bookRepo.GetByIdAsync(newBorrow.BookId);
                if (newBook is null)
                    return Result<int>.Fail("New book not found.", ErrorCodes.BookNotFound);

                // 5) Adjust stock safely
                if (oldBorrow.BookId == newBorrow.BookId)
                {
                    // Same book: adjust by the difference
                    var delta = newBorrow.Amount - oldBorrow.Amount; // positive means need more stock
                    if (delta > 0 && newBook.Amount < delta)
                        return Result<int>.Fail("Insufficient stock in the selected book.", ErrorCodes.StockInsufficient);

                    newBook.Amount -= delta; // delta could be negative (adds back stock)
                }
                else
                {
                    // Different books: return old, deduct new
                    oldBook.Amount += oldBorrow.Amount;

                    if (newBook.Amount < newBorrow.Amount)
                        return Result<int>.Fail("Insufficient stock in the selected book.", ErrorCodes.StockInsufficient);

                    newBook.Amount -= newBorrow.Amount;
                }

                // 6) Replace the borrow record
                borrowRepo.Remove(oldBorrow);

                var newEntity = _mapper.Map<Borrow>(newBorrow);
                await borrowRepo.AddAsync(newEntity);

                // 7) Persist all changes atomically
                var saved = await _unitOfWork.SaveChangesAsync() > 0;
                if (!saved)
                    return Result<int>.Fail("Failed to save changes.", ErrorCodes.DbSaveFailed);

                // NOTE: if Borrow has no surrogate Id and uses composite keys only,
                // consider returning a        // consider returning a DTO with the composite key instead of int.
                return Result<int>.Ok(newEntity.Id, "Borrow updated successfully.");
            }
            catch (OperationCanceledException)
            {
                return Result<int>.Fail("Operation was canceled.", ErrorCodes.Canceled);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Result<int>.Fail("Concurrency conflict while updating.", ErrorCodes.ConcurrencyError);
            }
            catch (AutoMapperMappingException)
            {
                return Result<int>.Fail("Data mapping failed.", ErrorCodes.MappingError);
            }
            catch (DbUpdateException)
            {
                return Result<int>.Fail("Database update failed during update.", ErrorCodes.DbUpdateError);
            }
            catch (Exception)
            {
                return Result<int>.Fail("An unexpected error occurred.", ErrorCodes.Unexpected);
            }
        }
        #endregion

        #region Delete

        //public async Task<bool> DeleteByKeyAsync(int userId, int bookId, DateTime dateBorrow)
        //{
        //    try
        //    {
        //        var borrowRepo = _unitOfWork.GetRepoartory<Borrow>();
        //        var bookRepo = _unitOfWork.GetRepoartory<Book>();

        //        // 1) هات السجل بالمفتاح المركّب (بدون Query)
        //        var list = await borrowRepo.GetAllAsync(b =>
        //            b.UserId == userId &&
        //            b.BookId == bookId &&
        //            b.DateBorrow == dateBorrow);

        //        var borrow = list.FirstOrDefault();
        //        if (borrow is null) return false; // NotFound

        //        // 2) هات الكتاب وارجع المخزون
        //        var book = await bookRepo.GetByIdAsync(borrow.BookId);
        //        if (book is null) throw new ArgumentException("Book not found.");

        //        // إعادة المخزون بالكمية المستعارة
        //        book.Amount += borrow.Amount;

        //        // 3) احذف السجل
        //        borrowRepo.Remove(borrow);

        //        // 4) حفظ
        //        return await _unitOfWork.SaveChangesAsync() > 0;
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        //            // لو فيه قيود FK تمنع الحذف (سياساتك)، سجّل السبب
        //        Console.WriteLine(ex.Message);
        //        return false;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}



        public async Task<Result<int>> DeleteByKeyAsync(int userId, int bookId, DateTime dateBorrow)
        {
            try
            {
                var borrowRepo = _unitOfWork.GetRepoartory<Borrow>();
                var bookRepo = _unitOfWork.GetRepoartory<Book>();

                // 1) Fetch the borrow by composite key
                var list = await borrowRepo.GetAllAsync(b =>
                    b.UserId == userId &&
                    b.BookId == bookId &&
                    b.DateBorrow == dateBorrow);

                var borrow = list.FirstOrDefault();
                if (borrow is null)
                    return Result<int>.Fail("Borrow record not found.", ErrorCodes.BorrowNotFound);

                // 2) Return stock to the book
                var book = await bookRepo.GetByIdAsync(borrow.BookId);
                if (book is null)
                    return Result<int>.Fail("Book not found.", ErrorCodes.BookNotFound);

                book.Amount += borrow.Amount;

                // 3) Delete the borrow
                borrowRepo.Remove(borrow);

                // 4) Persist
                var saved = await _unitOfWork.SaveChangesAsync() > 0;
                if (!saved)
                    return Result<int>.Fail("Failed to save changes.", ErrorCodes.DbSaveFailed, borrow.Id);

                return Result<int>.Ok(borrow.Id, "Borrow deleted successfully.");
            }
            catch (OperationCanceledException)
            {
                return Result<int>.Fail("Operation was canceled.", ErrorCodes.Canceled);
            }
            catch (DbUpdateException)
            {
                // Likely foreign key restriction or other DB-level issue
                return Result<int>.Fail("Database update failed during delete.", ErrorCodes.DbUpdateError);
            }
            catch (Exception)
            {
                return Result<int>.Fail("An unexpected error occurred.", ErrorCodes.Unexpected);
            }
        }
        #endregion
    }
}

