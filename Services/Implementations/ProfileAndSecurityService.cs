using LMS.API.Repositories.Interfaces;
using LMS.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LMS.API.Services.Implementations
{
    public class ProfileAndSecurityService : IProfileAndSecurityService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserDeviceRepository _deviceRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProfileAndSecurityService(
            IUserRepository userRepository,
            IUserDeviceRepository deviceRepository,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _deviceRepository = deviceRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return false;

            if (!VerifyPasswordHash(oldPassword, user.PasswordHash))
                return false;

            user.PasswordHash = HashPassword(newPassword);
            _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DisconnectDeviceAsync(int userId, int deviceId)
        {
            var device = await _deviceRepository.GetByIdAsync(deviceId);
            if (device == null || device.UserId != userId)
                return false;

            device.RefreshTokenHash = string.Empty;
            _deviceRepository.Delete(device);
            await _deviceRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DisconnectAllDevicesAsync(int userId)
        {
            var devices = await _deviceRepository.GetDevicesByUserIdAsync(userId);
            var deviceList = devices.ToList();

            if (!deviceList.Any())
                return false;

            foreach (var device in deviceList)
            {
                device.RefreshTokenHash = string.Empty;
                _deviceRepository.Delete(device);
            }

            await _deviceRepository.SaveChangesAsync();
            return true;
        }

        private static bool VerifyPasswordHash(string password, string storedHash)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var computedHash = Convert.ToBase64String(
                sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));
            return computedHash == storedHash;
        }

        private static string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            return Convert.ToBase64String(
                sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));
        }
    }
}
