namespace LMS.API.DTOs.Dashboard;

public class ActiveSessionsSummaryDto
{
    public int LiveNowCount { get; set; }
    public int UpcomingTodayCount { get; set; }
    public List<LiveSession.LiveSessionCardDto> TodaySessions { get; set; } = new();
}
