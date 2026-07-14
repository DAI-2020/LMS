using LMS.API.DTOs.Dashboard;

namespace LMS.API.Services.Interfaces;

public interface IDashboardService
{
    Task<TasksSummaryDto> GetTasksSummaryAsync(int studentId);
    Task<AttendanceSummaryDto> GetAttendanceSummaryAsync(int studentId);
    Task<IEnumerable<ActiveSessionsSummaryDto>> GetActiveSessionsAsync();
    Task<IEnumerable<GrowthAreaMetricDto>> GetGrowthAreasAsync(int studentId);
}
