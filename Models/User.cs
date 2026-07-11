using System.Net.Sockets;

namespace LMS.API.Models;

public class User
{
    public int Id { get; set; }

    public string FullName { get; set; }

    public string Email { get; set; }

    public string PasswordHash { get; set; }

    public DateTime CreatedAt { get; set; }

    //  Internal Relations
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    public ICollection<Course> Courses { get; set; } = new List<Course>();

    public ICollection<TaskSubmission> TaskSubmissions { get; set; } = new List<TaskSubmission>();

    public ICollection<AIAssistantChat> Chats { get; set; } = new List<AIAssistantChat>();
    
    //External Relations
    public ICollection<AttendanceLog> AttendanceLogs { get; set; } = new List<AttendanceLog>(); // 1->many relationship
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>(); // 1->many relationship

    public ICollection<CommunityPost> CommunityPosts { get; set; } = new List<CommunityPost>(); // 1->many relationship

}