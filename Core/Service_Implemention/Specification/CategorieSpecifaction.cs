using Domain_Layer.Models.Categories_Models;
using Shared;

namespace Service_Implemention.Specification
{
    public class CategorieSpecifaction : BaseSpecification<Categories>
    {
        public CategorieSpecifaction(CategorieQueryParamter categorieQuery) :
         base(U => string.IsNullOrEmpty(categorieQuery.search) || U.CategoryName.ToLower().Contains(categorieQuery.search.ToLower()))
        {
            ApplyPagention(categorieQuery.pageSize, categorieQuery.PageIndex);

        }
    }
}
