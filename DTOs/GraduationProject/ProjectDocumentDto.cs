namespace LMS.API.DTOs.GraduationProject;

public class ProjectDocumentDto
{
    public int DocumentId { get; set; }
    public string DocumentName { get; set; } = string.Empty;
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public string FileUrl { get; set; } = string.Empty;
}
