using LMS.API.Enums.AttendanceLog;

namespace LMS.API.Models;

public class AttendanceLog
{
    public int Id { get; set; }
    public int SessionId { get; set; } // Foreign key to Live_sessions
    public int StudentId { get; set; } // Foreign key to (Students)
    public DateTime? JoinTime { get; set; }
    public DateTime? LeaveTime { get; set; }
    public int MicrophoneUsageSeconds { get; set; } = 0;
    // مستويات التفاعل وحالة الحضور باستخدام الـ Enums
    public ParticipationLevel ParticipationLevel { get; set; }
    public AttendanceStatus Status { get; set; } // الحقل الجديد اللي هيربط مع الـ Dashboard (Attended, Missed, Passed)

    // درجة التفاعل الإجمالية (مثلاً من 100 وتحسب بالـ Logic البسيط)
    public int EngagementScore { get; set; }
    //Relationships

    public LiveSession LiveSession { get; set; } // علاقة Many-to-One مع LiveSessions
    public User Student { get; set; } // علاقة Many-to-One مع Students
}