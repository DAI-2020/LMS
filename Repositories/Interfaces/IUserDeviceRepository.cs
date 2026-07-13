using LMS.API.Models;

namespace LMS.API.Repositories.Interfaces
{
    public interface IUserDeviceRepository : IRepository<UserDevice>
    {
        Task<IEnumerable<UserDevice>> GetDevicesByUserIdAsync(int userId);
        Task<UserDevice?> GetByRefreshTokenHashAsync(string tokenHash);
        Task DisconnectDeviceAsync(UserDevice device);
        Task DisconnectAllDevicesAsync(int userId);
        Task SaveChangesAsync();
    }
}
