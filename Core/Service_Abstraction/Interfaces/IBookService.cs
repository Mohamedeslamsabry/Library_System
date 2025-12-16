using Shared;
using Shared.DTO.Book;
using Shared.Error;

namespace Service_Abstraction.Interfaces
{
    public interface IBookService
    {
        #region GetAllAsync
        Task<PaginatedResult<BookDTO>> GetAllAsync(BookQueryPartmer bookQuery);
        #endregion

        #region GetByIdAsync
        Task<BookDTO?> GetByIdAsync(int id);
        #endregion

        #region CreateAsync
        Task<Result<int>> CreateAsync(CreateOrUpdateBookDto createBook);
        #endregion

        #region UpdateAsync
        Task<Result<int>> UpdateAsync(int id, CreateOrUpdateBookDto updateBook);
        #endregion

        #region DeleteAsync
        Task<Result<int>> DeleteAsync(int id);
        #endregion
    }
}
