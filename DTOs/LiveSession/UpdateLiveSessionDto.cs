namespace LMS.API.DTOs.LiveSession
{
    public class UpdateLiveSessionDto
    {
        public int? CourseId { get; set; }
        public string? Title { get; set; }
        public DateTime? ScheduledAt { get; set; }
        public int? DurationMinutes { get; set; }
        public int? WeekNumber { get; set; }
        public string? Status { get; set; }
        public string? Type { get; set; }
        public string? Mode { get; set; }
        public string? RecordingUrl { get; set; }
    }
}
