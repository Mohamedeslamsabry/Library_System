using Shared.DTO.Floor;
using Shared.Error;

namespace Service_Abstraction.Interfaces
{
    public interface IFloorService
    {
        #region GetAllAsync
        Task<IEnumerable<FloorDTO>> GetAllAsync();
        #endregion

        #region GetByIdAsync
        Task<FloorDTO?> GetByIdAsync(int FloorNumber);
        #endregion

        #region CreateAsync
        Task<Result<int>> CreateAsync(CreateOrUpdateFloorDTO createFloor);
        #endregion

        #region UpdateAsync
        Task<Result<int>> UpdateAsync(int FloorNumber, CreateOrUpdateFloorDTO updateFloorDTO);
        #endregion

        #region DeleteAsync
        Task<Result<int>> DeleteAsync(int FloorNumber);
        #endregion
    }
}
