using LMS.API.Enums.TasksEnums;

namespace LMS.API.Models;

public class CourseTask
{
    public int Id { get; set; }

    public int CourseId { get; set; }
    public int? SessionId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public TaskType TaskType { get; set; }
    public AssignmentStatus AssignmentStatus  { get; set; }

    public DateTime DueDate { get; set; }
    public string AllowedExtensions{ get; set;}
    
    //internal relationships
    public Course Course { get; set; }
    public ICollection<TaskSubmission> Submissions { get; set; } = new List<TaskSubmission>();
    
    //external relationship
    public virtual LiveSession? LiveSession { get; set; }
}