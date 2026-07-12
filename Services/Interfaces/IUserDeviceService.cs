using LMS.API.DTOs.UserDevice;

namespace LMS.API.Services.Interfaces
{
    public interface IUserDeviceService
    {
        Task<IEnumerable<UserDeviceResponseDto>> GetUserDevicesAsync(int userId, string currentTokenHash);
        Task<bool> DisconnectDeviceAsync(int userId, int deviceId);
    }
}
