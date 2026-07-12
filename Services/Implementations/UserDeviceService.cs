using LMS.API.DTOs.UserDevice;
using LMS.API.Repositories.Interfaces;
using LMS.API.Services.Interfaces;

namespace LMS.API.Services.Implementations
{
    public class UserDeviceService : IUserDeviceService
    {
        private readonly IUserDeviceRepository _deviceRepository;

        public UserDeviceService(IUserDeviceRepository deviceRepository)
        {
            _deviceRepository = deviceRepository;
        }

        public async Task<IEnumerable<UserDeviceResponseDto>> GetUserDevicesAsync(int userId, string currentTokenHash)
        {
            var devices = await _deviceRepository.GetDevicesByUserIdAsync(userId);

            return devices.Select(d => new UserDeviceResponseDto
            {
                Id = d.Id,
                DeviceName = d.DeviceName,
                ClientInfo = d.ClientInfo,
                LastUsed = d.LastUsed,
                IsCurrentDevice = d.RefreshTokenHash == currentTokenHash
            });
        }

        public async Task<bool> DisconnectDeviceAsync(int userId, int deviceId)
        {
            var device = await _deviceRepository.GetByIdAsync(deviceId);

            if (device == null || device.UserId != userId)
            {
                return false;
            }

            await _deviceRepository.DisconnectDeviceAsync(device);
            return true;
        }
    }
}
