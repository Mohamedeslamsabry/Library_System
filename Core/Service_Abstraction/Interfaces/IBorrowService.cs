using Shared;
using Shared.DTO.Borrow;
using Shared.Error;

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
        Task<Result<int>> CreateAsync(CreateOrUpdateBorrowDTO createBorrow);
        #endregion

        #region UpdateAsync

        Task<Result<int>> UpdateByKeyAsync(int userId, int bookId, DateTime dateBorrow, CreateOrUpdateBorrowDTO updateBorrow);

        #endregion

        #region DeleteAsync

        Task<Result<int>> DeleteByKeyAsync(int userId, int bookId, DateTime dateBorrow);
        #endregion
    }
}
