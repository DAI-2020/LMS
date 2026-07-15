using System.ComponentModel.DataAnnotations;

namespace LMS.API.DTOs.AIAssistantChat
{
    public class SummarizeLessonDto
    {
        [Required]
        public int CourseId { get; set; }

        [Required]
        [MaxLength(10000)]
        public string LessonContent { get; set; } = string.Empty;
    }
}
