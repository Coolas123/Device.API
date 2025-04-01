using Domain.Primitives;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repository
{

    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : Entity
    {
        private readonly ApplicationDbContext dbContext;
        protected readonly DbSet<TEntity> dbSet;

        public BaseRepository(ApplicationDbContext dbContext) {
            this.dbContext = dbContext;
            dbSet = this.dbContext.Set<TEntity>();
        }

        public async Task CreateAsync(TEntity entity) {
            await dbSet.AddAsync(entity);
        }

        public virtual async Task<TEntity> GetByIdAsync(int id) {
            return await dbSet.FirstOrDefaultAsync(x => x.Id == id);
        }

        public void Update(TEntity entity) {
            dbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<TEntity> entities) {
            dbSet.UpdateRange(entities);
        }

        public void Delete(TEntity entity) {
            dbSet.Remove(entity);
        }
    }
}
