using Domain.Entities;

namespace Domain.Repository
{
    public interface IDeviceRepository : IBaseRepository<Device>
    {
        Task<IEnumerable<Device>> GetAllDevicesAsync();

        Task<bool> DeleteByIdAsync(int id);

        Task<bool> DeleteByTypeAsync(string type);
    }
}
