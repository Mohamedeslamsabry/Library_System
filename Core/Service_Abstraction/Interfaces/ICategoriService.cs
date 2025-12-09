using Shared;
using Shared.DTO.Categories;
using Shared.DTO.Employee;

namespace Service_Abstraction.Interfaces
{
    public interface ICategoriService
    {
        #region GetAllAsync
        Task<PaginatedResult<CategorieDTO>> GetAllAsync(CategorieQueryParamter categorieSearch);
        #endregion
    }
}
