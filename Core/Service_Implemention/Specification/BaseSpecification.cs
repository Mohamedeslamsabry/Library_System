using Domain_Layer.Contract.specfication;
using Domain_Layer.Models.Shared;
using System.Linq.Expressions;

namespace Service_Implemention.Specification
{
    public class BaseSpecification<TEntity> : ISpecification<TEntity> where TEntity : BaseEntity
    {
        #region set Criteria
        protected BaseSpecification(Expression<Func<TEntity, bool>> CriteriaExpersion)
        {
            Criteria = CriteriaExpersion;
        }
        #endregion

        public Expression<Func<TEntity, bool>> Criteria { get; private set; }



        #region Set Include
        //protected void AddInclude(Expression<Func<TEntity, object>> incudeExpression)
        //{
        //    IncudeExpression.Add(incudeExpression);
        //}
        #endregion

        #region Order By
        public Expression<Func<TEntity, object>> OrderBy { get; private set; }

        public Expression<Func<TEntity, object>> OrderByDesc { get; private set; }

        #endregion

        #region Order By (Set)

        protected void SetOrdery(Expression<Func<TEntity, object>> orderBy)
        {
            OrderBy = orderBy;
        }

        protected void SetOrderyDesc(Expression<Func<TEntity, object>> orderByDesc)
        {
            OrderByDesc = orderByDesc;
        }
        #endregion

        #region Pagention
        public int Skip { get; private set; }
        public int Take { get; private set; }
        public bool IsPaginate { get; set; }

        //                         100        10             3      
        protected void ApplyPagention(int PageSize, int PageIndex)
        {
            IsPaginate = true;
            Take = PageSize;
            Skip = (PageIndex - 1) * PageSize;
        }


        #endregion
    }
}
