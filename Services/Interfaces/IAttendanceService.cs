using LMS.API.DTOs.Attendance;

namespace LMS.API.Services.Interfaces
{
    public interface IAttendanceService
    {
        Task<bool> JoinSessionAsync(JoinSessionDto dto);
        Task<bool> LeaveSessionAsync(LeaveSessionDto dto);
        Task<StudentAttendanceSummaryDto?> GetStudentSummaryAsync(int studentId);
        Task<IEnumerable<AttendanceLogDto>> GetSessionAttendanceAsync(int sessionId);
        Task<AttendanceSummaryResponseDto> GetAttendanceSummaryAsync(int studentId, AttendanceFilterDto filter);
    }
}
