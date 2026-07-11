using LMS.API.Enums;
using LMS.API.Enums.TasksEnums;

namespace LMS.API.Models;

public class TaskSubmission
{
    public int Id { get; set; }

    public int TaskId { get; set; }

    public int StudentId { get; set; }

    public string FileUrl { get; set; }

    public DateTime SubmittedAt { get; set; }

    public AssignmentStatus AssignmentStatus { get; set; }

    public string? AIGrade { get; set; }

    public string? AIFeedback { get; set; }

    public CourseTask CourseTask { get; set; }

    public User Student { get; set; }
}