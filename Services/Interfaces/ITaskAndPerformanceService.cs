using LMS.API.DTOs.GrowthAreas;
using LMS.API.DTOs.TasksDashboard;

namespace LMS.API.Services.Interfaces;

public interface ITaskAndPerformanceService
{
    Task<TasksSummaryResponseDto> GetTasksSummaryAsync(int studentId, int courseId);

    Task<IEnumerable<GrowthAreaResponseDto>> GetGrowthAreasAsync(int studentId, int courseId);
}
