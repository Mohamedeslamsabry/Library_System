using Shared.DTO.Floor;

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
        Task<bool> CreateAsync(CreateOrUpdateFloorDTO createFloor);
        #endregion

        #region UpdateAsync
        Task<bool> UpdateAsync(int FloorNumber, CreateOrUpdateFloorDTO updateFloorDTO);
        #endregion

        #region DeleteAsync
        Task<bool> DeleteAsync(int FloorNumber);
        #endregion
    }
}
