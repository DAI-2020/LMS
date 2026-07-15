using LMS.API.Enums.GraduationProjectEnums;

namespace LMS.API.DTOs.GraduationProject;

public class GraduationProjectResponseDto
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public string ProjectName { get; set; } = string.Empty;

    public string LeadProject { get; set; } = string.Empty;

    public string DescriptionProject { get; set; } = string.Empty;

    public string UploadDocumentProject { get; set; } = string.Empty;

    public GraduationProjectStatus ProjectStatus { get; set; }

    public DateTime SubmittedAt { get; set; }
}
