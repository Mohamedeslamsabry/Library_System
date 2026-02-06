using Shared;
using Shared.DTO;
using Shared.DTO.authors;
using Shared.Error;

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
        Task<Result<int>> CreateAsync(CreateOrUpdateAuthorDTO createAuthor);
        #endregion

        #region UpdateAsync
        Task<Result<int>> UpdateAsync(int Id, CreateOrUpdateAuthorDTO updateAuthor);
        #endregion
    }
}
