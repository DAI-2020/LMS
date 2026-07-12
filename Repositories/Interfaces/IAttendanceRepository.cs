using LMS.API.Models;

namespace LMS.API.Repositories.Interfaces
{
    public interface IAttendanceRepository
    {
        // ميثود تجيب كل سجلات حضور الطالب مع المحاضرات بتاعتها عشان نفلتر عليها في السيرفس
        Task<IEnumerable<AttendanceLog>> GetStudentAttendanceWithSessionsAsync(int studentId);
    }
}
