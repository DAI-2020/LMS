namespace LMS.API.DTOs.Assignment;

public class AssignmentDetailsDto
{
    public int AssignmentId { get; set; }
    public string StatusLabel { get; set; } = string.Empty;
    public string HeaderDisplay { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public string TimeDisplayText { get; set; } = string.Empty;
    public string AllowedSubmissionType { get; set; } = string.Empty;
    public int MaxFileSizeInMB { get; set; } = 10;
}
