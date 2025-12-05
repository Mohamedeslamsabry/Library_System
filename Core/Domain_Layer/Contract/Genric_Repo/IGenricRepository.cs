using Domain_Layer.Contract.specfication;
using Domain_Layer.Models.Shared;
using System.Linq.Expressions;

namespace Domain_Layer.Contract.Genric_Repo
{
    public interface IGenricRepository<TEntity> where TEntity : BaseEntity
    {
        #region GetAll        
        Task<IEnumerable<TEntity>> GetAllAsync(Func<TEntity, bool>? Cretira = null);
        #endregion

        #region GetById
        Task<TEntity?> GetByIdAsync(int id);
        #endregion

        #region Update
        void Update(TEntity entity);
        #endregion

        #region Remove
        void Remove(TEntity entity);
        #endregion

        #region Add
        Task AddAsync(TEntity entity);
        #endregion

        #region AnyAsync
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default);

        #endregion   

        #region specification
        Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity> specification);

        Task<int> CountAsync(ISpecification<TEntity> specification);
        #endregion
    }
}
