namespace LMS.API.DTOs.LiveSession;

public class PaginatedSessionsRequestDto
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Mode { get; set; }
    public string? Status { get; set; }
    public string? Type { get; set; }
    public int? CourseId { get; set; }
    public string? Search { get; set; }
}
