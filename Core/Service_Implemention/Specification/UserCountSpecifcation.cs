using Domain_Layer.Models.Users_Models;
using Shared;

namespace Service_Implemention.Specification
{
    public class UserCountSpecifcation : BaseSpecification<Users>
    {
        public UserCountSpecifcation(UserQueryParamter userQuery) :
          base(U => string.IsNullOrEmpty(userQuery.search) || U.User_Name.ToLower().Contains(userQuery.search.ToLower()))
        {

        }
    }
}
