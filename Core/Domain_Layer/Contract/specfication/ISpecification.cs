using Domain_Layer.Models.Shared;
using System.Linq.Expressions;

namespace Domain_Layer.Contract.specfication
{
    public interface ISpecification<TEntity> where TEntity : BaseEntity
    {
        Expression<Func<TEntity, bool>> Criteria { get; } 
        //List<Expression<Func<TEntity, object>>> IncudeExpression { get; } 

        #region OrderBy
        Expression<Func<TEntity, object>> OrderBy { get; }
        Expression<Func<TEntity, object>> OrderByDesc { get; }
        #endregion

        #region Pagenation
        public int Skip { get; }
        public int Take { get; }
        public bool IsPaginate { get; set; }
        #endregion
    }
}
