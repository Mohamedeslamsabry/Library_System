using Domain_Layer.Models.Puplishers_Models;
using Shared;

namespace Service_Implemention.Specification
{
    public class PublisherCountSpecifcation : BaseSpecification<Puplishers>
    {
        public PublisherCountSpecifcation(PublisherQueryParamter publisherQuery) :
          base(U => string.IsNullOrEmpty(publisherQuery.search) || U.Publisher_Name.ToLower().Contains(publisherQuery.search.ToLower()))
        {

        }
    }
}
