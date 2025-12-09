using Shared;
using Shared.DTO.Book;

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
        Task<bool> CreateAsync(CreateOrUpdateBookDto createBook);
        #endregion

        #region UpdateAsync
        Task<bool> UpdateAsync(int id, CreateOrUpdateBookDto updateBook);
        #endregion

        #region DeleteAsync
        Task<bool> DeleteAsync(int id);
        #endregion
    }
}
