using LMS.API.DTOs.Attendance;
using LMS.API.Models;

namespace LMS.API.Repositories.Interfaces
{
    public interface IAttendanceLogRepository
    {
        Task<IEnumerable<AttendanceLog>> GetAllAsync();
        Task<AttendanceLog?> GetByIdAsync(int id);
        Task<IEnumerable<AttendanceLog>> GetBySessionIdAsync(int sessionId);
        Task<IEnumerable<AttendanceLog>> GetByStudentIdAsync(int studentId);
        Task<AttendanceLog?> GetAttendanceLogAsync(int sessionId, int studentId);
        Task<StudentAttendanceSummaryDto?> GetStudentSummaryAsync(int studentId);
        Task<AttendanceLog> AddAsync(AttendanceLog log);
        Task UpdateAsync(AttendanceLog log);
        Task DeleteAsync(int id);
    }
}
