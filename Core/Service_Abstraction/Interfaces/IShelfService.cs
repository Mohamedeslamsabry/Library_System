using Shared;
using Shared.DTO.Shelf;
using Shared.Error;

namespace Service_Abstraction.Interfaces
{
    public interface IShelfService
    {
        #region GetAllAsync
        Task<PaginatedResult<ShelfDTO>> GetAllAsync(ShelfQueryParamter shelfQuery);
        #endregion

        #region GetByIdAsync
        Task<ShelfDTO?> GetByIdAsync(int id);
        #endregion

        #region CreateAsync
        Task<Result<int>> CreateAsync(CreateOrUpdateShelfDTO CreateShelf);
        #endregion

        #region UpdateAsync
        Task<Result<int>> UpdateAsync(int Id, CreateOrUpdateShelfDTO UpdateShelf);
        #endregion

        #region DeleteAsync
        Task<Result<int>> DeleteAsync(int Id);
        #endregion
    }
}
