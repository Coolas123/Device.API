using Domain.Entities;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repository
{
    public class DeviceRepository : BaseRepository<Device>, IDeviceRepository
    {
        public DeviceRepository(ApplicationDbContext dbContext) : base(dbContext) {
        }

        public async Task<bool> DeleteByIdAsync(int id) {
            var deleteResult = await dbSet.Where(x => x.Id == id).ExecuteDeleteAsync();

            if (deleteResult == 0) {
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteByTypeAsync(string type) {
            var deleteResult =  await dbSet.Where(x=>x.Type == type).ExecuteDeleteAsync();

            if (deleteResult == 0) {
                return false;
            }

            return true;
        }

        public async Task<IEnumerable<Device>> GetAllDevicesAsync() {
            return await dbSet.Select(x=>x).ToArrayAsync();
        }
    }
}
