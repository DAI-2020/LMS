using System.ComponentModel.DataAnnotations;

namespace LMS.API.DTOs.GraduationProject;

public class SubmitGraduationProjectDto
{
    [Required]
    public int StudentId { get; set; }

    [Required]
    [MaxLength(200)]
    public string ProjectName { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string LeadProject { get; set; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string DescriptionProject { get; set; } = string.Empty;

    [Required]
    public IFormFile UploadDocument { get; set; }
}
