using System.ComponentModel.DataAnnotations;

namespace LMS.API.DTOs.AIAssistantChat
{
    public class StudyPlanDto
    {
        [Required]
        public int CourseId { get; set; }

        [Required]
        [MaxLength(5000)]
        public string AdditionalInfo { get; set; } = string.Empty;
    }
}
