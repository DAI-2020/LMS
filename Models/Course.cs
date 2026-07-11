namespace LMS.API.Models;

public class Course
{
    public int Id { get; set; }

    public string Title { get; set; }
    public string Description { get; set; }
    public int InstructorId { get; set; }

    public User Instructor { get; set; }

    //internal relations
    public ICollection<Material> Materials { get; set; } = new List<Material>();

    public ICollection<CourseTask> CourseTasks { get; set; } = new List<CourseTask>();

    public ICollection<AIAssistantChat> Chats { get; set; } = new List<AIAssistantChat>();

    //External relations 
    
    public ICollection<LiveSession> LiveSessions { get; set; } = new List<LiveSession>(); //1->many relationship
}