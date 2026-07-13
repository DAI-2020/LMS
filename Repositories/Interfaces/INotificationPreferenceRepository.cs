using LMS.API.Models;

namespace LMS.API.Repositories.Interfaces
{
    public interface INotificationPreferenceRepository : IRepository<NotificationPreferences>
    {
        Task<NotificationPreferences?> GetByUserIdAsync(int userId);
        Task SaveChangesAsync();
    }
}
