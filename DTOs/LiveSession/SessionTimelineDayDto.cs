namespace LMS.API.DTOs.LiveSession;

public class SessionTimelineDayDto
{
    public DateTime Date { get; set; }
    public string DayName { get; set; } = string.Empty;
    public int SessionCount { get; set; }
    public List<LiveSessionResponseDto> Sessions { get; set; } = new();
}
