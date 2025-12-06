using Domain_Layer.Contract.Genric_Repo;
using Domain_Layer.Contract.UnitOfWork;
using Domain_Layer.Models.Shared;
using Persistence.Data.DbContexts;

namespace Persistence.Implment_repo
{
    public class UnitOfWork(LibraryDbContext _dbContext) : IUnitOfWork
    {
        private readonly Dictionary<string, object> _repositry = [];

        public IGenricRepository<TEntity> GetRepoartory<TEntity>() where TEntity : BaseEntity
        {
            var TypeName = typeof(TEntity).Name;         
            if (_repositry.TryGetValue(TypeName, out object? value))
            {
                return (IGenricRepository<TEntity>)value;
            }
            else
            {
                var Repo = new GenricRepository<TEntity>(_dbContext);
                _repositry.Add(TypeName, Repo);
                return Repo;
            }
        }

        public async Task<int> SaveChangesAsync() => await _dbContext.SaveChangesAsync();
    }
}
