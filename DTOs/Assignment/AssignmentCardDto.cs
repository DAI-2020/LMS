namespace LMS.API.DTOs.Assignment;

public class AssignmentCardDto
{
    public int AssignmentId { get; set; }
    public string Status { get; set; } = string.Empty;
    public int WeekNumber { get; set; }
    public int SessionNumber { get; set; }
    public bool IsTechnical { get; set; }
    public string HeaderDisplay { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public string TimeDisplayText { get; set; } = string.Empty;
    public bool IsAttendanceCompleted { get; set; }
    public bool IsTaskCompleted { get; set; }
    public string ButtonState { get; set; } = string.Empty;
    public string ButtonText { get; set; } = string.Empty;
}
