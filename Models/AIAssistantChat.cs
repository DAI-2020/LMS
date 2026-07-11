namespace LMS.API.Models;

public class AIAssistantChat
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public int CourseId { get; set; }

    public string UserQuery { get; set; }

    public string AIResponse { get; set; }

    public DateTime AskedAt { get; set; }

    public User Student { get; set; }

    public Course Course { get; set; }
}