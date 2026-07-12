namespace LMS.API.Models
{
    public class NotificationPreferences
    {
        public int Id { get; set; }
        public string UserId { get; set; } // الربط مع جدول المستخدمين IdentityUser

        // حقول الـ Switches الثنائية
        public bool NewLessons { get; set; } = true; // القيمة الافتراضية تفعيل
        public bool LiveSessionReminders { get; set; } = true;
        public bool AssignmentDeadlines { get; set; } = true;
        public bool AssignmentGrading { get; set; } = true;
        public bool QuizAlerts { get; set; } = true;
        public bool CommunityNotifications { get; set; } = true;
        public bool AiRecommendations { get; set; } = false; // قد يفضل البعض غلقها افتراضياً
        public bool SecurityAlerts { get; set; } = true; // يفضل أن تكون إجبارية أو مفعلة دائماً لأسباب أمنية

        // Navigation Property (اختياري حسب تصميم الـ Auth عندك)
         public User User { get; set; }
    }
}
