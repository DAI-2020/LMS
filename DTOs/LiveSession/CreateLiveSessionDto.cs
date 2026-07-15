using System.ComponentModel.DataAnnotations;

namespace LMS.API.DTOs.LiveSession
{
    public class CreateLiveSessionDto
    {
        public int CourseId { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public DateTime ScheduledAt { get; set; }
        public int DurationMinutes { get; set; }
        [Required]
        public string Type { get; set; } = string.Empty;
        [Required]
        public string Mode { get; set; } = string.Empty;
    }
}
