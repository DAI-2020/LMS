using LMS.API.Models;

namespace LMS.API.Repositories.Interfaces
{
    public interface INotificationPreferenceRepository
    {
        Task<NotificationPreferences?> GetByUserIdAsync(int userId);
        Task AddAsync(NotificationPreferences preferences);
        void Update(NotificationPreferences preferences);
        Task SaveChangesAsync();
    }
}
