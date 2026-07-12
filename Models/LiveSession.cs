using LMS.API.Enums.LiveSession;
namespace LMS.API.Models;


public class LiveSession
{
    public int Id { get; set; }
    public int CourseId { get; set; } // Foreign key to Course
    public string Title { get; set; } //(عنوان المحاضرة - مثل: "Session 1: Intro to C#")
    public DateTime ScheduledAt { get; set; } //(تاريخ ووقت المحاضرة)
    public int DurationMinutes { get; set; }       // مدة المحاضرة (مثلاً 180 لـ 3 ساعات)
    public LiveSessionStatus Status { get; set; }
    public int WeekNumber { get; set; }          // رقم الأسبوع الذي تنتمي إليه المحاضرة (مثلاً 1، 2، 3، ... إلخ)
    public LiveSessionType Type { get; set; }          // Technical أو Non-Technical
    public DeliveryMode Mode { get; set; }          // Online أو Onsite
    public string? RecordingUrl { get; set; }      // لينك الفيديو المسجل للحصة بعد انتهائها
    //Relationships


    //بين Course و LiveSession (علاقة One-to-Many)
    public Course Course { get; set; }
    public ICollection<AttendanceLog> AttendanceLogs { get; set; } = new List<AttendanceLog>(); // علاقة One-to-Many مع AttendanceLog
    public virtual ICollection<Material> Materials { get; set; } = new List<Material>(); // علاقة One-to-Many مع Materials
}
