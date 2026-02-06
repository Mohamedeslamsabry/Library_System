using Domain_Layer.Models.Authors_Models;
using Shared;

namespace Service_Implemention.Specification
{
    public class AuthorsCountSpecifaction : BaseSpecification<Authors>
    {
        public AuthorsCountSpecifaction(AuthorsQueryParamter authorsQuery) :
        base(U => string.IsNullOrEmpty(authorsQuery.search) || U.Auth_Name.ToLower().Contains(authorsQuery.search.ToLower()))
        {

        }
    }
}
