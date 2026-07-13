using LMS.API.Enums.GraduationProjectEnums;

namespace LMS.API.DTOs.GraduationProject;

public class GraduationProjectResponseDto
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public string ProjectName { get; set; }

    public string LeadProject { get; set; }

    public string DescriptionProject { get; set; }

    public string UploadDocumentProject { get; set; }

    public GraduationProjectStatus ProjectStatus { get; set; }

    public DateTime SubmittedAt { get; set; }
}
