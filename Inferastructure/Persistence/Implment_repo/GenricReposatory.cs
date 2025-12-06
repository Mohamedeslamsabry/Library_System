using Domain_Layer.Contract.Genric_Repo;
using Domain_Layer.Contract.specfication;
using Domain_Layer.Models.Shared;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.DbContexts;
using Persistence.specifcation;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Persistence.Implment_repo
{
    public class GenricRepository<TEntity>(LibraryDbContext _dbContext) : IGenricRepository<TEntity> where TEntity : BaseEntity
    {
        #region GetAllAsync
        async Task<IEnumerable<TEntity>> IGenricRepository<TEntity>.GetAllAsync(Func<TEntity, bool>? Cretira)
        {
            if (Cretira is null)
            {
                return await _dbContext.Set<TEntity>().ToListAsync();
            }
            else
            {
                return _dbContext.Set<TEntity>().Where(Cretira).ToList();
            }
        }


        #endregion

        #region GetByIdAsync
        public async Task<TEntity?> GetByIdAsync(int id) => await _dbContext.Set<TEntity>().FindAsync(id);

        #endregion

        #region AddAsync
        public async Task AddAsync(TEntity entity) => await _dbContext.Set<TEntity>().AddAsync(entity);

        #endregion

        #region Update
        public void Update(TEntity entity) => _dbContext.Set<TEntity>().Update(entity);

        #endregion

        #region Remove
        public void Remove(TEntity entity) => _dbContext.Set<TEntity>().Remove(entity);


        #endregion

        #region AnyAsync
        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
        {
            // AsNoTracking مهم للأداء في استعلامات القراءة
            return _dbContext.Set<TEntity>().AsNoTracking().AnyAsync(predicate, ct);
        }
        #endregion

        #region specification
        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity> specification)
        {
            return await SpecificationElavautor.CreateQuery(_dbContext.Set<TEntity>(), specification).ToListAsync();
        }

        public async Task<int> CountAsync(ISpecification<TEntity> specification)
        {
            return await SpecificationElavautor.CreateQuery(_dbContext.Set<TEntity>(), specification).CountAsync();
        }

        //public async Task<TEntity?> GetByIdAsync(ISpecification<TEntity> specification)
        //{
        //    return await SpecificationElavautor.CreateQuery(_dbContext.Set<TEntity>(), specification).FirstOrDefaultAsync();
        //}
        #endregion

    }
}
