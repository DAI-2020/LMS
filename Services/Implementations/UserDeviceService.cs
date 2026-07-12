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

            // عمل Mapping مانيوال للـ DTO ومعادلة تحديد الجهاز الحالي النشط
            return devices.Select(d => new UserDeviceResponseDto
            {
                Id = d.Id,
                DeviceName = d.DeviceName,
                ClientInfo = d.ClientInfo,
                LastUsed = d.LastUsed,
                IsCurrentDevice = d.RefreshTokenHash == currentTokenHash // لو الـ Token هو اللي داخل بيه حالياً، يعلم عليه True
            });
        }

        public async Task<bool> DisconnectDeviceAsync(int userId, int deviceId)
        {
            var device = await _deviceRepository.GetByIdAsync(deviceId);

            // تأكيد أمان: نتأكد إن الجهاز موجود وبيخص المستخدم نفسه اللي باعت الطلب من الـ API
            if (device == null || device.UserId != userId)
            {
                return false;
            }

            _deviceRepository.Delete(device);
            await _deviceRepository.SaveChangesAsync();
            return true;
        }
    }
}
