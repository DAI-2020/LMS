namespace LMS.API.Models
{
    public class NotificationPreferences
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public bool NewLessons { get; set; } = true;
        public bool LiveSessionReminders { get; set; } = true;
        public bool AssignmentDeadlines { get; set; } = true;
        public bool AssignmentGrading { get; set; } = true;
        public bool QuizAlerts { get; set; } = true;
        public bool CommunityNotifications { get; set; } = true;
        public bool AiRecommendations { get; set; } = false;
        public bool SecurityAlerts { get; set; } = true;

        public User User { get; set; } = null!;
    }
}
