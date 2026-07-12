using LMS.API.DTOs.Attendance;

namespace LMS.API.Services.Interfaces
{
    public interface IAttendanceSummaryService
    {
        Task<AttendanceSummaryResponseDto> GetAttendanceSummaryAsync(int studentId, AttendanceFilterDto filter);
        Task<double> GetCumulativeAttendancePercentageAsync(int studentId, string? sessionType);
    }
}
