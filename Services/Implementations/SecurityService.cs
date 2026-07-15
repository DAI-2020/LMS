using LMS.API.DTOs.Security;
using LMS.API.Repositories.Interfaces;
using LMS.API.Services.Interfaces;

namespace LMS.API.Services.Implementations;

public class SecurityService : ISecurityService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserDeviceService _deviceService;
    private readonly IProfileAndSecurityService _profileService;

    public SecurityService(
        IUnitOfWork unitOfWork,
        IUserDeviceService deviceService,
        IProfileAndSecurityService profileService)
    {
        _unitOfWork = unitOfWork;
        _deviceService = deviceService;
        _profileService = profileService;
    }

    public async Task<SecuritySettingsSummaryDto> GetSummaryAsync(int userId, string currentTokenHash)
    {
        var devices = await _deviceService.GetUserDevicesAsync(userId, currentTokenHash);

        return new SecuritySettingsSummaryDto
        {
            IsTwoStepVerificationEnabled = false,
            SecurityQuestion = "What is your pet's name?",
            ActiveDevices = devices.Select(d => new ActiveDeviceDto
            {
                SessionId = d.Id,
                DeviceType = d.DeviceName,
                BrowserAndOs = d.ClientInfo ?? "Unknown",
                LastActiveDateTime = d.LastUsed,
                LastActiveText = GetRelativeTime(d.LastUsed),
                IsCurrentSession = d.IsCurrentDevice
            }).ToList()
        };
    }

    public async Task<bool> UpdateSettingsAsync(int userId, UpdateSecuritySettingsDto dto)
    {
        if (!string.IsNullOrEmpty(dto.NewPassword))
        {
            var result = await _profileService.ChangePasswordAsync(userId, dto.CurrentPassword, dto.NewPassword);
            if (!result) return false;
        }
        return true;
    }

    private static string GetRelativeTime(DateTime dateTime)
    {
        var diff = DateTime.UtcNow - dateTime;
        if (diff.TotalMinutes < 1) return "Just now";
        if (diff.TotalMinutes < 60) return $"{(int)diff.TotalMinutes}m ago";
        if (diff.TotalHours < 24) return $"{(int)diff.TotalHours}h ago";
        return $"{(int)diff.TotalDays}d ago";
    }
}
