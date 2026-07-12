namespace LMS.API.DTOs.LiveSession
{
    public class LiveSessionResponseDto
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string Title { get; set; }
        public DateTime ScheduledAt { get; set; }
        public int DurationMinutes { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string Mode { get; set; }
        public int WeekNumber { get; set; }
        public string? RecordingUrl { get; set; }
        public int AttendanceCount { get; set; }
    }
}
