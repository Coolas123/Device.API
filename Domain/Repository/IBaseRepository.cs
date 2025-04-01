using Domain.Primitives;

namespace Domain.Repository
{
    public interface IBaseRepository<TEntity> where TEntity : Entity
    {
        Task CreateAsync(TEntity entity);

        Task<TEntity> GetByIdAsync(int id);

        void Update(TEntity entity);

        void Delete(TEntity entity);
    }
}
