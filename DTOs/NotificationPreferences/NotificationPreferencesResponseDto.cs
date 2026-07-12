namespace LMS.API.DTOs.NotificationPreferences
{
    public class NotificationPreferencesResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool NewLessons { get; set; }
        public bool LiveSessionReminders { get; set; }
        public bool AssignmentDeadlines { get; set; }
        public bool AssignmentGrading { get; set; }
        public bool QuizAlerts { get; set; }
        public bool CommunityNotifications { get; set; }
        public bool AiRecommendations { get; set; }
        public bool SecurityAlerts { get; set; }
    }
}
