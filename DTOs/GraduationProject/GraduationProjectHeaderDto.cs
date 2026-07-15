namespace LMS.API.DTOs.GraduationProject;

public class GraduationProjectHeaderDto
{
    public bool HasData { get; set; }
    public List<ProjectDocumentDto> UploadedDocuments { get; set; } = new();
}
