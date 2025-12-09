using Shared;
using Shared.DTO.Borrow;

namespace Service_Abstraction.Interfaces
{
    public interface IBorrowService
    {
        #region GetAllAsync
        Task<PaginatedResult<BorrowDTO>> GetAllAsync(BorrowQueryParamter borrowQuery);
        #endregion

        #region GetByIdAsync
        Task<BorrowDTO?> GetByIdAsync(int userId, int bookId, DateTime dateBorrow);
        #endregion

        #region CreateAsync
        Task<bool> CreateAsync(CreateOrUpdateBorrowDTO createBorrow);
        #endregion

        #region UpdateAsync

        Task<bool> UpdateByKeyAsync(int userId, int bookId, DateTime dateBorrow, CreateOrUpdateBorrowDTO updateBorrow);

        #endregion

        #region DeleteAsync

        Task<bool> DeleteByKeyAsync(int userId, int bookId, DateTime dateBorrow);
        #endregion
    }
}
