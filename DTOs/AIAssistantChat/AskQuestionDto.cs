using System.ComponentModel.DataAnnotations;

namespace LMS.API.DTOs.AIAssistantChat
{
    public class AskQuestionDto
    {
        [Required]
        public int StudentId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        [MaxLength(5000)]
        public string UserQuery { get; set; }
    }
}
