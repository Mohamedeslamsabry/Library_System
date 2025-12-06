using Domain_Layer.Contract.specfication;
using Domain_Layer.Models.Shared;
using Microsoft.EntityFrameworkCore;

namespace Persistence.specifcation
{
    public static class SpecificationElavautor
    {
        public static IQueryable<TEntity> CreateQuery<TEntity>(IQueryable<TEntity> StartQuery, ISpecification<TEntity> specification) where TEntity : BaseEntity
        {
            var Query = StartQuery;
            if (specification.Criteria is not null)
            {
                Query = Query.Where(specification.Criteria);
            }

            if (specification.OrderBy is not null)
            {
                Query = Query.OrderBy(specification.OrderBy);
            }

            if (specification.OrderByDesc is not null)
            {
                Query = Query.OrderByDescending(specification.OrderByDesc);
            }

            //if (specification.IncudeExpression is not null && specification.IncudeExpression.Count() > 0)
            //{
            //    Query = specification.IncudeExpression.Aggregate(Query, (Current, IncludeExp) => Current.Include(IncludeExp));

            //}
            if (specification.IsPaginate)
            {
                Query = Query.Skip(specification.Skip).Take(specification.Take);
            }
            return Query;
        }

    }
}
