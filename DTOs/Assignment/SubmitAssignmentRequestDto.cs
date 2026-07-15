using System.ComponentModel.DataAnnotations;

namespace LMS.API.DTOs.Assignment;

public class SubmitAssignmentRequestDto
{
    [Required]
    public int AssignmentId { get; set; }

    [Required]
    public string SubmissionType { get; set; } = string.Empty;

    [Required]
    public IFormFile File { get; set; } = null!;
}
