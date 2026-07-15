using System.ComponentModel.DataAnnotations;

namespace LMS.API.DTOs.AIAssistantChat;

public class AiChatRequestDto
{
    public int StudentId { get; set; }

    [Required]
    public int CourseId { get; set; }

    [Required]
    [MaxLength(5000)]
    public string Message { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Feature { get; set; } = string.Empty;
}
