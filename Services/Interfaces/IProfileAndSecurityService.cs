namespace LMS.API.Services.Interfaces
{
    public interface IProfileAndSecurityService
    {
        Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword);
        Task<bool> DisconnectDeviceAsync(int userId, int deviceId);
        Task<bool> DisconnectAllDevicesAsync(int userId);
    }
}
