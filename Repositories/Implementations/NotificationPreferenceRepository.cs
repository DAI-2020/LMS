using LMS.API.Data;
using LMS.API.Models;
using LMS.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS.API.Repositories.Implementations
{
    public class NotificationPreferenceRepository : INotificationPreferenceRepository
    {
        private readonly LMSDbContext _context;

        public NotificationPreferenceRepository(LMSDbContext context)
        {
            _context = context;
        }

        public async Task<NotificationPreferences?> GetByUserIdAsync(int userId)
        {
            return await _context.Notifications
                .FirstOrDefaultAsync(n => n.UserId == userId);
        }

        public async Task AddAsync(NotificationPreferences preferences)
        {
            await _context.Notifications.AddAsync(preferences);
        }

        public void Update(NotificationPreferences preferences)
        {
            _context.Notifications.Update(preferences);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
