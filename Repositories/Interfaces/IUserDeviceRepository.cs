using LMS.API.Models;

namespace LMS.API.Repositories.Interfaces
{
    public interface IUserDeviceRepository
    {
        Task<UserDevice?> GetByIdAsync(int id);
        Task<IEnumerable<UserDevice>> GetDevicesByUserIdAsync(int userId);
        Task AddAsync(UserDevice device);
        void Delete(UserDevice device);
        Task<UserDevice?> GetByRefreshTokenHashAsync(string tokenHash);
        Task DisconnectDeviceAsync(UserDevice device);
        Task DisconnectAllDevicesAsync(int userId);
        Task SaveChangesAsync();
    }
}
