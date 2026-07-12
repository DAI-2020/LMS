using LMS.API.Data;
using LMS.API.Models;
using LMS.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS.API.Repositories.Implementations
{
    public class UserDeviceRepository : IUserDeviceRepository
    {
        private readonly LMSDbContext _context;

        public UserDeviceRepository(LMSDbContext context)
        {
            _context = context;
        }

        public async Task<UserDevice?> GetByIdAsync(int id)
        {
            return await _context.UserDevices.FindAsync(id);
        }

        public async Task<IEnumerable<UserDevice>> GetDevicesByUserIdAsync(int userId)
        {
            return await _context.UserDevices
                .Where(d => d.UserId == userId)
                .OrderByDescending(d => d.LastUsed)
                .ToListAsync();
        }

        public async Task AddAsync(UserDevice device)
        {
            await _context.UserDevices.AddAsync(device);
        }

        public void Delete(UserDevice device)
        {
            _context.UserDevices.Remove(device);
        }

        public async Task<UserDevice?> GetByRefreshTokenHashAsync(string tokenHash)
        {
            return await _context.UserDevices
                .FirstOrDefaultAsync(d => d.RefreshTokenHash == tokenHash);
        }

        public async Task DisconnectDeviceAsync(UserDevice device)
        {
            device.RefreshTokenHash = string.Empty;
            _context.UserDevices.Update(device);
            await _context.SaveChangesAsync();
        }

        public async Task DisconnectAllDevicesAsync(int userId)
        {
            var devices = await _context.UserDevices
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
