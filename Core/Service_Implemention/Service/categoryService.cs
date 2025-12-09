using AutoMapper;
using Domain_Layer.Contract.UnitOfWork;
using Domain_Layer.Models.Categories_Models;
using Domain_Layer.Models.Employee_Models;
using Service_Abstraction.Interfaces;
using Service_Implemention.Specification;
using Shared;
using Shared.DTO.Categories;
using Shared.DTO.Employee;

namespace Service_Implemention.Service
{
    public class categoryService(IUnitOfWork _unitOfWork , IMapper _mapper) : ICategoriService
    {
        #region GetAllAsync
        public async Task<PaginatedResult<CategorieDTO>> GetAllAsync(CategorieQueryParamter categorieSearch)
        {
            var Specification = new CategorieSpecifaction(categorieSearch);
            var Categories = await _unitOfWork.GetRepoartory<Categories>().GetAllAsync(Specification);
            var categoryDto = _mapper.Map<IEnumerable<Categories>, IEnumerable<CategorieDTO>>(Categories);

            #region Paggention
            var spec = new CategoriesCountSpecifaction(categorieSearch);
            var TotalCount = await _unitOfWork.GetRepoartory<Categories>().CountAsync(spec);
            #endregion

            return new PaginatedResult<CategorieDTO>(TotalCount, Categories.Count(), categorieSearch.PageIndex, categoryDto);
        }
        #endregion
    }
}
