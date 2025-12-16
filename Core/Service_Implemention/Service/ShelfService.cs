using AutoMapper;
using Domain_Layer.Contract.UnitOfWork;
using Domain_Layer.Exceptions;
using Domain_Layer.Models.Floors_Models;
using Domain_Layer.Models.Shelf_Models;
using Microsoft.EntityFrameworkCore;
using Service_Abstraction.Interfaces;
using Shared.DTO.Shelf;
using Shared.Error;

namespace Service_Implemention.Service
{
    public class ShelfService(IUnitOfWork _UnitOfWork, IMapper _mapper) : IShelfService
    {
        #region GetAllAsync
        public async Task<IEnumerable<ShelfDTO>> GetAllAsync()
        {
            var Shelf = await _UnitOfWork.GetRepoartory<Shelf>().GetAllAsync();
            if (Shelf is null)
            {
                return Enumerable.Empty<ShelfDTO>();
            }
            else
            {
                return _mapper.Map<IEnumerable<Shelf>, IEnumerable<ShelfDTO>>(Shelf);
            }
        }

        #endregion

        #region GetByIdAsync
        public async Task<ShelfDTO?> GetByIdAsync(int Id)
        {
            var Shelf = await _UnitOfWork.GetRepoartory<Shelf>().GetByIdAsync(Id);
            return Shelf == null ? throw new ShelfNotFoundException(Id) : _mapper.Map<ShelfDTO>(Shelf);
        }
        #endregion

        #region CreateAsync
        //public async Task<bool> CreateAsync(CreateOrUpdateShelfDTO CreateShelf)
        //{
        //    try
        //    {
        //        var Shelf = _mapper.Map<CreateOrUpdateShelfDTO, Shelf>(CreateShelf);
        //        // تأكد أن الدور موجود
        //        var floorExists = await _UnitOfWork.GetRepoartory<Floors>().AnyAsync(f => f.Id == CreateShelf.FloorNumber);
        //        if (!floorExists)
        //            throw new ArgumentException($"Floor Number ({CreateShelf.FloorNumber}) Not Found.", nameof(CreateShelf.FloorNumber));


        //        await _UnitOfWork.GetRepoartory<Shelf>().AddAsync(Shelf);
        //        var isCreated = await _UnitOfWork.SaveChangesAsync() > 0;
        //        if (!isCreated)
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            return isCreated;
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        return false;
        //    }
        //}

        public async Task<Result<int>> CreateAsync(CreateOrUpdateShelfDTO createShelf)
        {
            if (createShelf is null)
                return Result<int>.Fail("Request body is missing.", ErrorCodes.ValidationNull);

            try
            {
                // Validate Floor exists
                var floorExists = await _UnitOfWork.GetRepoartory<Floors>()
                    .AnyAsync(f => f.Id == createShelf.FloorNumber);
                if (!floorExists)
                    return Result<int>.Fail("Floor not found.", ErrorCodes.FloorNotFound);

                var shelf = _mapper.Map<CreateOrUpdateShelfDTO, Shelf>(createShelf);

                await _UnitOfWork.GetRepoartory<Shelf>().AddAsync(shelf);

                var saved = await _UnitOfWork.SaveChangesAsync() > 0;
                if (!saved)
                    return Result<int>.Fail("Failed to save changes.", ErrorCodes.DbSaveFailed);

                return Result<int>.Ok(shelf.Id, "Shelf created successfully.");
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
                return Result<int>.Fail("Database update failed during create.", ErrorCodes.DbUpdateError);
            }
            catch (Exception)
            {
                return Result<int>.Fail("Unexpected error occurred.", ErrorCodes.Unexpected);
            }
        }

        #endregion

        #region Update Shelf
        //public async Task<bool> UpdateAsync(int Id, CreateOrUpdateShelfDTO UpdateShelf)
        //{
        //    try
        //    {
        //        var Repo = _UnitOfWork.GetRepoartory<Shelf>();
        //        var Shelf = await Repo.GetByIdAsync(Id);
        //        if (Shelf is null) { return false; }

        //        // تأكد أن الدور موجود
        //        var floorExists = await _UnitOfWork.GetRepoartory<Floors>().AnyAsync(f => f.Id == UpdateShelf.FloorNumber);
        //        if (!floorExists)
        //            throw new ArgumentException($"Floor Number ({UpdateShelf.FloorNumber}) Not Found.", nameof(UpdateShelf.FloorNumber));

        //        _mapper.Map(UpdateShelf, Shelf);
        //        Repo.Update(Shelf);
        //        return await _UnitOfWork.SaveChangesAsync() > 0;
        //    }
        //    catch (Exception)
        //    {

        //        return false;
        //    }
        //}


        public async Task<Result<int>> UpdateAsync(int id, CreateOrUpdateShelfDTO updateShelf)
        {
            if (updateShelf is null)
                return Result<int>.Fail("Request body is missing.", ErrorCodes.ValidationNull);

            try
            {
                var repo = _UnitOfWork.GetRepoartory<Shelf>();
                var shelf = await repo.GetByIdAsync(id);

                if (shelf is null)
                    return Result<int>.Fail("Shelf not found.", ErrorCodes.ShelfNotFound);

                // Validate Floor exists
                var floorExists = await _UnitOfWork.GetRepoartory<Floors>()
                    .AnyAsync(f => f.Id == updateShelf.FloorNumber);
                if (!floorExists)
                    return Result<int>.Fail("Floor not found.", ErrorCodes.FloorNotFound);

                _mapper.Map(updateShelf, shelf);
                repo.Update(shelf);

                var saved = await _UnitOfWork.SaveChangesAsync() > 0;
                if (!saved)
                    return Result<int>.Fail("Failed to save changes.", ErrorCodes.DbSaveFailed);

                return Result<int>.Ok(shelf.Id, "Shelf updated successfully.");
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
                return Result<int>.Fail("Unexpected error occurred.", ErrorCodes.Unexpected);
            }
        }

        #endregion

        #region Delete Shelf
        //public async Task<bool> DeleteAsync(int Id)
        //{
        //    try
        //    {
        //        var Shelf = await _UnitOfWork.GetRepoartory<Shelf>().GetByIdAsync(Id);
        //        if (Shelf is null) { return false; }

        //        if (Shelf.Book.Any())
        //        {
        //            foreach (var Book in Shelf.Book)
        //            {
        //                Book.ShelfId = null;
        //            }
        //        }

        //        _UnitOfWork.GetRepoartory<Shelf>().Remove(Shelf);
        //        var IsRemoved = await _UnitOfWork.SaveChangesAsync() > 0;
        //        return IsRemoved;
        //    }
        //    catch (Exception)
        //    {

        //        return false;
        //    }
        //}


        public async Task<Result<int>> DeleteAsync(int id)
        {
            try
            {
                var shelfRepo = _UnitOfWork.GetRepoartory<Shelf>();
                var shelf = await shelfRepo.GetByIdAsync(id);

                if (shelf is null)
                    return Result<int>.Fail("Shelf not found.", ErrorCodes.ShelfNotFound);

                if (shelf.Book?.Any() == true)
                {
                    foreach (var book in shelf.Book)
                        book.ShelfId = null;
                }

                shelfRepo.Remove(shelf);

                var saved = await _UnitOfWork.SaveChangesAsync() > 0;
                if (!saved)
                    return Result<int>.Fail("Failed to save changes.", ErrorCodes.DbSaveFailed, id);

                return Result<int>.Ok(id, "Shelf deleted successfully.");
            }
            catch (OperationCanceledException)
            {
                return Result<int>.Fail("Operation was canceled.", ErrorCodes.Canceled);
            }
            catch (DbUpdateException)
            {
                return Result<int>.Fail("Database update failed during delete.", ErrorCodes.DbUpdateError);
            }
            catch (Exception)
            {
                return Result<int>.Fail("Unexpected error occurred.", ErrorCodes.Unexpected);
            }
        }
        #endregion
    }
}
