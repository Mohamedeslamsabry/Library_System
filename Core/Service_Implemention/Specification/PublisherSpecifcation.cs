using Domain_Layer.Models.Puplishers_Models;
using Shared;

namespace Service_Implemention.Specification
{
    public class PublisherSpecifcation : BaseSpecification<Puplishers>
    {
        public PublisherSpecifcation(PublisherQueryParamter publisherQuery) :
          base(U => string.IsNullOrEmpty(publisherQuery.search) || U.Publisher_Name.ToLower().Contains(publisherQuery.search.ToLower()))
        {
            ApplyPagention(publisherQuery.pageSize, publisherQuery.PageIndex);

        }
    }
}
