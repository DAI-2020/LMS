using LMS.API.DTOs.NotificationPreferences;

namespace LMS.API.Services.Interfaces
{
    public interface INotificationPreferenceService
    {
        Task<NotificationPreferencesResponseDto?> GetByUserIdAsync(int userId);
        Task<NotificationPreferencesResponseDto?> UpdateAsync(int userId, UpdateNotificationPreferencesDto dto);
    }
}
