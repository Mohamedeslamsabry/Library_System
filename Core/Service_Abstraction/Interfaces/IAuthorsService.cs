using Shared;
using Shared.DTO.authors;
using Shared.DTO.Publisher;

namespace Service_Abstraction.Interfaces
{
    public interface IAuthorsService
    {
        #region GetAllAsync
        Task<PaginatedResult<AuthorDTO>> GetAllAsync(AuthorsQueryParamter authorsQuery);
        #endregion

        #region GetByIdAsync
        Task<AuthorDTO?> GetByIdAsync(int Id);
        #endregion

        #region CreateAsync
        Task<bool> CreateAsync(CreateOrUpdateAuthorDTO createAuthor);
        #endregion

        #region UpdateAsync
        Task<bool> UpdateAsync(int Id, CreateOrUpdateAuthorDTO updateAuthor);
        #endregion

    }
}
