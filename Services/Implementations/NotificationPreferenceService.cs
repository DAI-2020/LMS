using LMS.API.DTOs.NotificationPreferences;
using LMS.API.Models;
using LMS.API.Repositories.Interfaces;
using LMS.API.Services.Interfaces;

namespace LMS.API.Services.Implementations
{
    public class NotificationPreferenceService : INotificationPreferenceService
    {
        private readonly INotificationPreferenceRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public NotificationPreferenceService(
            INotificationPreferenceRepository repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<NotificationPreferencesResponseDto?> GetByUserIdAsync(int userId)
        {
            var prefs = await _repository.GetByUserIdAsync(userId);
            if (prefs is null) return null;

            return MapToResponse(prefs);
        }

        public async Task<NotificationPreferencesResponseDto?> UpdateAsync(
            int userId, UpdateNotificationPreferencesDto dto)
        {
            var prefs = await _repository.GetByUserIdAsync(userId);

            if (prefs is null)
            {
                prefs = new NotificationPreferences
                {
                    UserId = userId,
                    NewLessons = dto.NewLessons ?? true,
                    LiveSessionReminders = dto.LiveSessionReminders ?? true,
                    AssignmentDeadlines = dto.AssignmentDeadlines ?? true,
                    AssignmentGrading = dto.AssignmentGrading ?? true,
                    QuizAlerts = dto.QuizAlerts ?? true,
                    CommunityNotifications = dto.CommunityNotifications ?? true,
                    AiRecommendations = dto.AiRecommendations ?? false,
                    SecurityAlerts = dto.SecurityAlerts ?? true
                };
                await _repository.AddAsync(prefs);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                if (dto.NewLessons.HasValue) prefs.NewLessons = dto.NewLessons.Value;
                if (dto.LiveSessionReminders.HasValue) prefs.LiveSessionReminders = dto.LiveSessionReminders.Value;
                if (dto.AssignmentDeadlines.HasValue) prefs.AssignmentDeadlines = dto.AssignmentDeadlines.Value;
                if (dto.AssignmentGrading.HasValue) prefs.AssignmentGrading = dto.AssignmentGrading.Value;
                if (dto.QuizAlerts.HasValue) prefs.QuizAlerts = dto.QuizAlerts.Value;
                if (dto.CommunityNotifications.HasValue) prefs.CommunityNotifications = dto.CommunityNotifications.Value;
                if (dto.AiRecommendations.HasValue) prefs.AiRecommendations = dto.AiRecommendations.Value;
                if (dto.SecurityAlerts.HasValue) prefs.SecurityAlerts = dto.SecurityAlerts.Value;

                _repository.Update(prefs);
                await _unitOfWork.SaveChangesAsync();
            }

            return MapToResponse(prefs);
        }

        private static NotificationPreferencesResponseDto MapToResponse(NotificationPreferences prefs)
        {
            return new NotificationPreferencesResponseDto
            {
                Id = prefs.Id,
                UserId = prefs.UserId,
                NewLessons = prefs.NewLessons,
                LiveSessionReminders = prefs.LiveSessionReminders,
                AssignmentDeadlines = prefs.AssignmentDeadlines,
                AssignmentGrading = prefs.AssignmentGrading,
                QuizAlerts = prefs.QuizAlerts,
                CommunityNotifications = prefs.CommunityNotifications,
                AiRecommendations = prefs.AiRecommendations,
                SecurityAlerts = prefs.SecurityAlerts
            };
        }
    }
}
