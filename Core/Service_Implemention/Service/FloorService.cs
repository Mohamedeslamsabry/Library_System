using AutoMapper;
using Domain_Layer.Contract.UnitOfWork;
using Domain_Layer.Models.Employee_Models;
using Domain_Layer.Models.Floors_Models;
using Domain_Layer.Models.Shelf_Models;
using Service_Abstraction.Interfaces;
using Shared.DTO.Floor;

namespace Service_Implemention.Service
{
    public class FloorService(IUnitOfWork _UnitOfWork, IMapper _mapper) : IFloorService
    {
        #region GetAllAsync
        public async Task<IEnumerable<FloorDTO>> GetAllAsync()
        {
            var Floors = await _UnitOfWork.GetRepoartory<Floors>().GetAllAsync();
            if (Floors is null)
            {
                return Enumerable.Empty<FloorDTO>();
            }
            else
            {
                return _mapper.Map<IEnumerable<Floors>, IEnumerable<FloorDTO>>(Floors);
            }
        }

        #endregion

        #region GetByIdAsync
        public async Task<FloorDTO?> GetByIdAsync(int FloorNumber)
        {
            var Floor = await _UnitOfWork.GetRepoartory<Floors>().GetByIdAsync(FloorNumber);
            return Floor == null ? null : _mapper.Map<FloorDTO>(Floor);
        }
        #endregion

        #region Create Floor
        public async Task<bool> CreateAsync(CreateOrUpdateFloorDTO createFloor)
        {
            try
            {
                var Floor = _mapper.Map<CreateOrUpdateFloorDTO, Floors>(createFloor);

                // شرط التحقق من وجود المدير
                if (createFloor.ManagerId.HasValue)
                {
                    var managerExists = await _UnitOfWork.GetRepoartory<Employee>().AnyAsync(e => e.Id == createFloor.ManagerId.Value);
                    if (!managerExists)
                        throw new ArgumentException($"This Employee By ({createFloor.ManagerId}) Not Found.", nameof(createFloor.ManagerId));
                }

                await _UnitOfWork.GetRepoartory<Floors>().AddAsync(Floor);
                var isCreated = await _UnitOfWork.SaveChangesAsync() > 0;
                if (!isCreated)
                {
                    return false;
                }
                else
                {
                    return isCreated;
                }

            }
            catch (Exception)
            {

                return false;
            }
        }

        #endregion

        #region Update Floor
        public async Task<bool> UpdateAsync(int FloorNumber, CreateOrUpdateFloorDTO updateFloorDTO)
        {
            try
            {
                var Repo = _UnitOfWork.GetRepoartory<Floors>();
                var Floor = await Repo.GetByIdAsync(FloorNumber);
                if (Floor is null) { return false; }

                // شرط التحقق من وجود المدير
                if (updateFloorDTO.ManagerId.HasValue)
                {
                    var managerExists = await _UnitOfWork.GetRepoartory<Employee>().AnyAsync(e => e.Id == updateFloorDTO.ManagerId.Value);
                    if (!managerExists)
                        throw new ArgumentException($"This Employee By ({updateFloorDTO.ManagerId}) Not Found.", nameof(updateFloorDTO.ManagerId));
                }

                _mapper.Map(updateFloorDTO, Floor);
                Repo.Update(Floor);
                return await _UnitOfWork.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {

                return false;
            }
        }

        #endregion

        #region Delete Floor
        public async Task<bool> DeleteAsync(int FloorNumber)
        {
            try
            {
                var Floor = await _UnitOfWork.GetRepoartory<Floors>().GetByIdAsync(FloorNumber);
                if (Floor is null) { return false; }

                //Bussniess Role
                if (Floor.employeesWork.Any())
                {
                    foreach (var Employee in Floor.employeesWork)
                    {
                        Employee.FloorsNumber = null;
                    }
                }

                if (Floor.Shelfs.Any())
                {
                    foreach (var Shelf in Floor.Shelfs)
                    {
                        Shelf.FloorNumber = null;
                    }
                }

                _UnitOfWork.GetRepoartory<Floors>().Remove(Floor);
                var IsRemoved = await _UnitOfWork.SaveChangesAsync() > 0;
                return IsRemoved;
            }
            catch (Exception)
            {

                return false;
            }
        }
        #endregion

    }
}
