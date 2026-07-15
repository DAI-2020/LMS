using LMS.API.DTOs.Account;

namespace LMS.API.Services.Interfaces;

public interface IAccountDetailsService
{
    Task<AccountDetailsDto?> GetDetailsAsync(int userId);
    Task<List<AccountDeviceDto>> GetDevicesAsync(int userId, string currentTokenHash);
    Task<bool> UpdateDetailsAsync(int userId, UpdateAccountDetailsRequestDto dto);
    Task<bool> DisconnectDeviceAsync(int userId, int deviceId);
}
