using LMS.API.Data;
using LMS.API.Models;
using LMS.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS.API.Repositories.Implementations
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly LMSDbContext _context;

        public AttendanceRepository(LMSDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AttendanceLog>> GetStudentAttendanceWithSessionsAsync(int studentId)
        {
            return await _context.AttendanceLogs
                .Include(al => al.LiveSession) 
                .Where(al => al.StudentId == studentId)
                .ToListAsync();
        }
    }
}
