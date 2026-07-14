namespace LMS.API.DTOs.LiveSession;

public class SessionTimelineResponseDto
{
    public int Year { get; set; }
    public int Month { get; set; }
    public string MonthName { get; set; } = string.Empty;
    public List<SessionTimelineDayDto> Days { get; set; } = new();
    public int TotalSessions { get; set; }
}
