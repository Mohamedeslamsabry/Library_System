using Domain_Layer.Models.Employee_Models;
using Domain_Layer.Models.Users_Models;
using Shared;

namespace Service_Implemention.Specification
{
    public class UserSpecification : BaseSpecification<Users>
    {
        public UserSpecification(UserQueryParamter userQuery) :
           base(U => string.IsNullOrEmpty(userQuery.search) || U.User_Name.ToLower().Contains(userQuery.search.ToLower()))
        {
            ApplyPagention(userQuery.pageSize, userQuery.PageIndex);

        }

    }
}
