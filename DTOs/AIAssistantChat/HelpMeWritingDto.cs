using System.ComponentModel.DataAnnotations;

namespace LMS.API.DTOs.AIAssistantChat;

public class HelpMeWritingDto
{
    [Required]
    public int CourseId { get; set; }

    [Required]
    [MaxLength(5000)]
    public string Text { get; set; } = string.Empty;
}
