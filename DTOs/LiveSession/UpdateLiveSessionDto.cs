namespace LMS.API.DTOs.LiveSession
{
    public class UpdateLiveSessionDto
    {
        public string? Title { get; set; }
        public DateTime? ScheduledAt { get; set; }
        public int? DurationMinutes { get; set; }
        public string? Status { get; set; }
        public string? Type { get; set; }
        public string? Mode { get; set; }
        public string? RecordingUrl { get; set; }
    }
}
