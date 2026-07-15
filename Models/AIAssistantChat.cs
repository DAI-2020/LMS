namespace LMS.API.Models;

public class AIAssistantChat
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public int CourseId { get; set; }

    public string UserQuery { get; set; } = string.Empty;

    public string AIResponse { get; set; } = string.Empty;

    public DateTime AskedAt { get; set; }

    public User Student { get; set; } = null!;

    public Course Course { get; set; } = null!;
}