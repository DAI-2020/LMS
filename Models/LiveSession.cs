using LMS.API.Enums.LiveSession;
namespace LMS.API.Models;


public class LiveSession
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime ScheduledAt { get; set; }
    public int DurationMinutes { get; set; }
    public LiveSessionStatus Status { get; set; }
    public int WeekNumber { get; set; }
    public LiveSessionType Type { get; set; }
    public DeliveryMode Mode { get; set; }
    public string? RecordingUrl { get; set; }
    public int InstructorId { get; set; }

    //Relationships
    public Course Course { get; set; } = null!;
    public User Instructor { get; set; } = null!;
    public ICollection<AttendanceLog> AttendanceLogs { get; set; } = new List<AttendanceLog>();
    public virtual ICollection<Material> Materials { get; set; } = new List<Material>();
}
