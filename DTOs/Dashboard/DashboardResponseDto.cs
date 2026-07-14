namespace LMS.API.DTOs.Dashboard;

public class DashboardResponseDto
{
    public TasksSummaryDto Tasks { get; set; } = new();
    public AttendanceSummaryDto Attendance { get; set; } = new();
    public ActiveSessionsSummaryDto ActiveSessions { get; set; } = new();
    public List<GrowthAreaMetricDto> GrowthAreas { get; set; } = new();
}
