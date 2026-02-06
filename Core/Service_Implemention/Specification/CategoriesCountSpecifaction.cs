using Domain_Layer.Models.Categories_Models;
using Shared;

namespace Service_Implemention.Specification
{
    public class CategoriesCountSpecifaction : BaseSpecification<Categories>
    {
        public CategoriesCountSpecifaction(CategorieQueryParamter categorieQuery) :
         base(U => string.IsNullOrEmpty(categorieQuery.search) || U.CategoryName.ToLower().Contains(categorieQuery.search.ToLower()))
        {

        }
    }
}
