using AutoMapper;
using Domain_Layer.Contract.UnitOfWork;
using Domain_Layer.Exceptions;
using Domain_Layer.Models.Employee_Models;
using Domain_Layer.Models.Floors_Models;
using Microsoft.EntityFrameworkCore;
using Service_Abstraction.Interfaces;
using Service_Implemention.Specification;
using Shared;
using Shared.DTO.Employee;
using Shared.DTO.Floor;
using Shared.Error;

namespace Service_Implemention.Service
{
    public class FloorService(IUnitOfWork _UnitOfWork, IMapper _mapper) : IFloorService
    {
        #region GetAllAsync
        public async Task<PaginatedResult<FloorDTO>> GetAllAsync(FloorQueryParamter floorQuery)
        {
            var Specification = new FloorSpecifcation(floorQuery);
            var Floors = await _UnitOfWork.GetRepoartory<Floors>().GetAllAsync(Specification);
            var FloorDto = _mapper.Map<IEnumerable<Floors>, IEnumerable<FloorDTO>>(Floors);

            #region Paggention
            var spec = new FloorSpecifcation(floorQuery);
            var TotalCount = await _UnitOfWork.GetRepoartory<Floors>().CountAsync(spec);
            #endregion

            return new PaginatedResult<FloorDTO>(TotalCount, Floors.Count(), floorQuery.PageIndex, FloorDto);
        }

        #endregion

        #region GetByIdAsync
        public async Task<FloorDTO?> GetByIdAsync(int FloorNumber)
        {
            var Floor = await _UnitOfWork.GetRepoartory<Floors>().GetByIdAsync(FloorNumber);
            return Floor == null ? throw new FloorNotFoundException(FloorNumber) : _mapper.Map<FloorDTO>(Floor);
        }
        #endregion

        #region Create Floor
        //public async Task<bool> CreateAsync(CreateOrUpdateFloorDTO createFloor)
        //{
        //    try
        //    {
        //        var Floor = _mapper.Map<CreateOrUpdateFloorDTO, Floors>(createFloor);

        //        // شرط التحقق من وجود المدير
        //        if (createFloor.ManagerId.HasValue)
        //        {
        //            var managerExists = await _UnitOfWork.GetRepoartory<Employee>().AnyAsync(e => e.Id == createFloor.ManagerId.Value);
        //            if (!managerExists)
        //                throw new ArgumentException($"This Employee By ({createFloor.ManagerId}) Not Found.", nameof(createFloor.ManagerId));
        //        }

        //        await _UnitOfWork.GetRepoartory<Floors>().AddAsync(Floor);
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

        public async Task<Result<int>> CreateAsync(CreateOrUpdateFloorDTO createFloor)
        {
            if (createFloor is null)
                return Result<int>.Fail("Request body is missing.", ErrorCodes.ValidationNull);

            try
            {
                if (createFloor.ManagerId.HasValue)
                {
                    var managerExists = await _UnitOfWork.GetRepoartory<Employee>()
                    .AnyAsync(e => e.Id == createFloor.ManagerId.Value);

                    if (!managerExists)
                        return Result<int>.Fail("Manager employee not found.", ErrorCodes.ManagerNotFound);
                }

                var floor = _mapper.Map<CreateOrUpdateFloorDTO, Floors>(createFloor);

                await _UnitOfWork.GetRepoartory<Floors>().AddAsync(floor);
                var saved = await _UnitOfWork.SaveChangesAsync() > 0;

                if (!saved)
                    return Result<int>.Fail("Failed to save changes.", ErrorCodes.DbSaveFailed);

                return Result<int>.Ok(floor.Id, "Floor created successfully.");
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
                return Result<int>.Fail("Database update failed during create This could be because the manager of this floor is the same manager as another floor.", ErrorCodes.DbUpdateError);
            }
            catch (Exception)
            {
                return Result<int>.Fail("Unexpected error occurred.", ErrorCodes.Unexpected);
            }
        }

        #endregion

        #region Update Floor
        //public async Task<bool> UpdateAsync(int FloorNumber, CreateOrUpdateFloorDTO updateFloorDTO)
        //{
        //    try
        //    {
        //        var Repo = _UnitOfWork.GetRepoartory<Floors>();
        //        var Floor = await Repo.GetByIdAsync(FloorNumber);
        //        if (Floor is null) { return false; }

        //        // شرط التحقق من وجود المدير
        //        if (updateFloorDTO.ManagerId.HasValue)
        //        {
        //            var managerExists = await _UnitOfWork.GetRepoartory<Employee>().AnyAsync(e => e.Id == updateFloorDTO.ManagerId.Value);
        //            if (!managerExists)
        //                throw new ArgumentException($"This Employee By ({updateFloorDTO.ManagerId}) Not Found.", nameof(updateFloorDTO.ManagerId));
        //        }

        //        _mapper.Map(updateFloorDTO, Floor);
        //        Repo.Update(Floor);
        //        return await _UnitOfWork.SaveChangesAsync() > 0;
        //    }
        //    catch (Exception)
        //    {

        //        return false;
        //    }
        //}

        public async Task<Result<int>> UpdateAsync(int floorNumber, CreateOrUpdateFloorDTO updateFloorDTO)
        {
            if (updateFloorDTO is null)
                return Result<int>.Fail("Request body is missing.", ErrorCodes.ValidationNull);

            try
            {
                var repo = _UnitOfWork.GetRepoartory<Floors>();
                var floor = await repo.GetByIdAsync(floorNumber);

                if (floor is null)
                    return Result<int>.Fail("Floor not found.", ErrorCodes.FloorNotFound);

                if (updateFloorDTO.ManagerId.HasValue)
                {
                    var managerExists = await _UnitOfWork.GetRepoartory<Employee>()
                        .AnyAsync(e => e.Id == updateFloorDTO.ManagerId.Value);

                    if (!managerExists)
                        return Result<int>.Fail("Manager employee not found.", ErrorCodes.ManagerNotFound);
                }

                _mapper.Map(updateFloorDTO, floor);
                repo.Update(floor);

                var saved = await _UnitOfWork.SaveChangesAsync() > 0;

                if (!saved)
                    return Result<int>.Fail("Failed to save changes.", ErrorCodes.DbSaveFailed);

                return Result<int>.Ok(floor.Id, "Floor updated successfully.");
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
                return Result<int>.Fail("Database update failed during update This could be because the manager of this floor is the same manager as another floor.", ErrorCodes.DbUpdateError);
            }
            catch (Exception)
            {
                return Result<int>.Fail("Unexpected error occurred.", ErrorCodes.Unexpected);
            }
        }

        #endregion

        #region Delete Floor

        //public async Task<bool> DeleteAsync(int FloorNumber)
        //{
        //    try
        //    {
        //        var Floor = await _UnitOfWork.GetRepoartory<Floors>().GetByIdAsync(FloorNumber);
        //        if (Floor is null) { return false; }

        //        //Bussniess Role Employee
        //        if (Floor.employeesWork.Any())
        //        {
        //            foreach (var Employee in Floor.employeesWork)
        //            {
        //                Employee.FloorsNumber = null;
        //            }
        //        }

        //        if (Floor.Shelfs.Any())
        //        {
        //            foreach (var Shelf in Floor.Shelfs)
        //            {
        //                Shelf.FloorNumber = null;
        //            }
        //        }

        //        _UnitOfWork.GetRepoartory<Floors>().Remove(Floor);
        //        var IsRemoved = await _UnitOfWork.SaveChangesAsync() > 0;
        //        return IsRemoved;
        //    }
        //    catch (Exception)
        //    {

        //        return false;
        //    }
        //}


        public async Task<Result<int>> DeleteAsync(int floorNumber)
        {
            try
            {
                var floorRepo = _UnitOfWork.GetRepoartory<Floors>();
                var floor = await floorRepo.GetByIdAsync(floorNumber);

                if (floor is null)
                    return Result<int>.Fail("Floor not found.", ErrorCodes.FloorNotFound);

                // Unlink employees (set FK to null)
                if (floor.employeesWork?.Any() == true)
                {
                    foreach (var employee in floor.employeesWork)
                        employee.FloorsNumber = null;
                }

                // Unlink shelves (set FK to null)
                if (floor.Shelfs?.Any() == true)
                {
                    foreach (var shelf in floor.Shelfs)
                        shelf.FloorNumber = null;
                }

                floorRepo.Remove(floor);

                var saved = await _UnitOfWork.SaveChangesAsync() > 0;
                if (!saved)
                    return Result<int>.Fail("Failed to save changes.", ErrorCodes.DbSaveFailed, floor.Id);


                return Result<int>.Ok(floor.Id, "Floor deleted successfully.");
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
