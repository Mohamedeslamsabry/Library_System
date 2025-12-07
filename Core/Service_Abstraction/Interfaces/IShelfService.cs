using Shared.DTO.Shelf;

namespace Service_Abstraction.Interfaces
{
    public interface IShelfService
    {
        #region GetAllAsync
        Task<IEnumerable<ShelfDTO>> GetAllAsync();
        #endregion

        #region GetByIdAsync
        Task<ShelfDTO?> GetByIdAsync(int id);
        #endregion

        #region CreateAsync
        Task<bool> CreateAsync(CreateOrUpdateShelfDTO CreateShelf);
        #endregion

        #region UpdateAsync
        Task<bool> UpdateAsync(int Id, CreateOrUpdateShelfDTO UpdateShelf);
        #endregion

        #region DeleteAsync
        Task<bool> DeleteAsync(int Id);
        #endregion
    }
}
