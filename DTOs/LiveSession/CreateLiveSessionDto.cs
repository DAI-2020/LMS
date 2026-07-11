namespace LMS.API.DTOs.LiveSession
{
    public class CreateLiveSessionDto
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public DateTime ScheduledAt { get; set; }
        public int DurationMinutes { get; set; }
        public string Type { get; set; }
        public string Mode { get; set; }
    }
}
