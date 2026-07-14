namespace LMS.API.DTOs.LiveSession;

public class LiveSessionCardDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string CourseName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Mode { get; set; } = string.Empty;
    public DateTime ScheduledAt { get; set; }
    public int DurationMinutes { get; set; }
    public bool HasAttendance { get; set; }
    public bool HasTask { get; set; }
    public bool HasSurvey { get; set; }
    public string? RecordingUrl { get; set; }
    public string ActionLabel { get; set; } = string.Empty;
    public int AttendanceCount { get; set; }
}
