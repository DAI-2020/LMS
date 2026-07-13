using LMS.API.Data;
using LMS.API.Models;
using LMS.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS.API.Repositories.Implementations
{
    public class UserDeviceRepository : Repository<UserDevice>, IUserDeviceRepository
    {
        public UserDeviceRepository(LMSDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<UserDevice>> GetDevicesByUserIdAsync(int userId)
        {
            return await _dbSet
                .Where(d => d.UserId == userId)
                .OrderByDescending(d => d.LastUsed)
                .ToListAsync();
        }

        public async Task<UserDevice?> GetByRefreshTokenHashAsync(string tokenHash)
        {
            return await _dbSet
                .FirstOrDefaultAsync(d => d.RefreshTokenHash == tokenHash);
        }

        public async Task DisconnectDeviceAsync(UserDevice device)
        {
            device.RefreshTokenHash = string.Empty;
            _dbSet.Update(device);
            await _context.SaveChangesAsync();
        }

        public async Task DisconnectAllDevicesAsync(int userId)
        {
            var devices = await _dbSet
                .Where(d => d.UserId == userId)
                .ToListAsync();

            foreach (var device in devices)
            {
                device.RefreshTokenHash = string.Empty;
            }

            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
