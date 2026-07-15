using System.ComponentModel.DataAnnotations;

namespace LMS.API.DTOs.GraduationProject;

public class SubmitProjectDocumentDto
{
    [Required]
    public int ProjectNameId { get; set; }

    [Required]
    public int LeadProjectId { get; set; }

    [MaxLength(2000)]
    public string? Description { get; set; }

    [Required]
    public IFormFile File { get; set; } = null!;
}
