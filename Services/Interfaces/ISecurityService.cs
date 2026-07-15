using LMS.API.DTOs.Security;

namespace LMS.API.Services.Interfaces;

public interface ISecurityService
{
    Task<SecuritySettingsSummaryDto> GetSummaryAsync(int userId, string currentTokenHash);
    Task<bool> UpdateSettingsAsync(int userId, UpdateSecuritySettingsDto dto);
}
