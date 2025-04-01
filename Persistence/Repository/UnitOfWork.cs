using Domain.Repository;

namespace Persistence.Repository
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext dbContext;

        public UnitOfWork(ApplicationDbContext dbContext) {
            this.dbContext = dbContext;
        }

        public async Task SaveChangesAsync() {
            await dbContext.SaveChangesAsync();
        }
    }
}
