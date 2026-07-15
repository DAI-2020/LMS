namespace LMS.API.DTOs.GraduationProject;

public class ProjectUploadMetadataDto
{
    public List<LookupDto> ProjectNames { get; set; } = new();
    public List<LookupDto> ProjectLeads { get; set; } = new();
}

public class LookupDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
