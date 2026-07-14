namespace LMS.API.DTOs.LiveSession;

public class SessionTimelineFilterDto
{
    public int? Year { get; set; }
    public int? Month { get; set; }
    public int? WeekNumber { get; set; }
    public string? Category { get; set; }
    public string? Mode { get; set; }
    public int? CourseId { get; set; }
}
