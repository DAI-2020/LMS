using Microsoft.EntityFrameworkCore;
using LMS.API.Data;
using LMS.API.DTOs.Attendance;
using LMS.API.Enums.AttendanceLog;
using LMS.API.Models;
using LMS.API.Repositories.Interfaces;

namespace LMS.API.Repositories.Implementations
{
    public class AttendanceLogRepository : IAttendanceLogRepository
    {
        private readonly LMSDbContext _context;

        public AttendanceLogRepository(LMSDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AttendanceLog>> GetAllAsync()
        {
            return await _context.AttendanceLogs
                .Include(al => al.LiveSession)
                .Include(al => al.Student)
                .OrderByDescending(al => al.JoinTime)
                .ToListAsync();
        }

        public async Task<AttendanceLog?> GetByIdAsync(int id)
        {
            return await _context.AttendanceLogs
                .Include(al => al.LiveSession)
                .Include(al => al.Student)
                .FirstOrDefaultAsync(al => al.Id == id);
        }

        public async Task<IEnumerable<AttendanceLog>> GetBySessionIdAsync(int sessionId)
        {
            return await _context.AttendanceLogs
                .Include(al => al.Student)
                .Where(al => al.SessionId == sessionId)
                .ToListAsync();
        }

        public async Task<IEnumerable<AttendanceLog>> GetByStudentIdAsync(int studentId)
        {
            return await _context.AttendanceLogs
                .Include(al => al.LiveSession)
                .Where(al => al.StudentId == studentId)
                .OrderByDescending(al => al.JoinTime)
                .ToListAsync();
        }

        public async Task<AttendanceLog?> GetAttendanceLogAsync(int sessionId, int studentId)
        {
            return await _context.AttendanceLogs
                .FirstOrDefaultAsync(al => al.SessionId == sessionId && al.StudentId == studentId);
        }

        public async Task<StudentAttendanceSummaryDto?> GetStudentSummaryAsync(int studentId)
        {
            var logs = await _context.AttendanceLogs
                .Where(al => al.StudentId == studentId)
                .ToListAsync();

            if (!logs.Any()) return null;

            return new StudentAttendanceSummaryDto
            {
                StudentId = studentId,
                TotalSessions = logs.Count,
                AttendedSessions = logs.Count(al => al.Status == AttendanceStatus.Attend),
                MissedSessions = logs.Count(al => al.Status == AttendanceStatus.Missed),
                PassedSessions = logs.Count(al => al.Status == AttendanceStatus.Passed),
                AttendancePercentage = Math.Round((double)logs.Count(al => al.Status != AttendanceStatus.Missed) / logs.Count * 100, 2),
                AverageEngagementScore = Math.Round(logs.Average(al => al.EngagementScore), 2)
            };
        }

        public async Task<AttendanceLog> AddAsync(AttendanceLog log)
        {
            await _context.AttendanceLogs.AddAsync(log);
            await _context.SaveChangesAsync();
            return log;
        }

        public async Task UpdateAsync(AttendanceLog log)
        {
            _context.AttendanceLogs.Update(log);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var log = await _context.AttendanceLogs.FindAsync(id);
            if (log is not null)
            {
                _context.AttendanceLogs.Remove(log);
                await _context.SaveChangesAsync();
            }
        }
    }
}
