using Domain_Layer.Contract.Genric_Repo;
using Domain_Layer.Models.Shared;

namespace Domain_Layer.Contract.UnitOfWork
{
    public interface IUnitOfWork
    {
        IGenricRepository<TEntity> GetRepoartory<TEntity>() where TEntity : BaseEntity;

        Task<int> SaveChangesAsync();   
    }
}
