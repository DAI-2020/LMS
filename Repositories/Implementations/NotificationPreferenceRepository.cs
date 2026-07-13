using LMS.API.Data;
using LMS.API.Models;
using LMS.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS.API.Repositories.Implementations
{
    public class NotificationPreferenceRepository : Repository<NotificationPreferences>, INotificationPreferenceRepository
    {
        public NotificationPreferenceRepository(LMSDbContext context)
            : base(context)
        {
        }

        public async Task<NotificationPreferences?> GetByUserIdAsync(int userId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(n => n.UserId == userId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
