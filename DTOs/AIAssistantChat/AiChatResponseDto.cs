namespace LMS.API.DTOs.AIAssistantChat;

public class AiChatResponseDto
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public int CourseId { get; set; }

    public string UserQuery { get; set; } = string.Empty;

    public string AIResponse { get; set; } = string.Empty;

    public DateTime AskedAt { get; set; }
}
