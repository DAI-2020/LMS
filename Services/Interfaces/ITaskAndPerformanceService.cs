using LMS.API.DTOs.Dashboard;

namespace LMS.API.Services.Interfaces
{
    public interface ITaskAndPerformanceService
    {
        Task<TasksSummaryResponseDto> GetTasksSummaryAsync(int courseId);

        Task<IEnumerable<GrowthAreaResponseDto>> GetGrowthAreasAsync(int studentId, int courseId);
    }
}
