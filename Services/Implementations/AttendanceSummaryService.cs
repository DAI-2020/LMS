using LMS.API.DTOs.Attendance;
using LMS.API.Enums.AttendanceLog;
using LMS.API.Enums.LiveSession;
using LMS.API.Repositories.Interfaces;
using LMS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS.API.Services.Implementations
{
    public class AttendanceSummaryService : IAttendanceSummaryService
    {
        private readonly IAttendanceLogRepository _attendanceRepo;

        public AttendanceSummaryService(IAttendanceLogRepository attendanceRepo)
        {
            _attendanceRepo = attendanceRepo;
        }

        public async Task<AttendanceSummaryResponseDto> GetAttendanceSummaryAsync(int studentId, AttendanceFilterDto filter)
        {
            var query = _attendanceRepo.GetStudentAttendanceWithSessionsQuery(studentId);

            if (!string.IsNullOrEmpty(filter.SessionType))
            {
                if (Enum.TryParse<LiveSessionType>(filter.SessionType, true, out var parsedType))
                {
                    query = query.Where(al => al.LiveSession.Type == parsedType);
                }
            }

            if (!string.IsNullOrEmpty(filter.TimeRange))
            {
                var now = DateTime.UtcNow;
                if (filter.TimeRange.Equals("ThisMonth", StringComparison.OrdinalIgnoreCase))
                {
                    var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
                    query = query.Where(al => al.LiveSession.ScheduledAt >= firstDayOfMonth);
                }
                else if (filter.TimeRange.Equals("ThisYear", StringComparison.OrdinalIgnoreCase))
                {
                    var firstDayOfYear = new DateTime(now.Year, 1, 1);
                    query = query.Where(al => al.LiveSession.ScheduledAt >= firstDayOfYear);
                }
            }

            var total = await query.CountAsync();
            var attended = await query.CountAsync(al => al.Status == AttendanceStatus.Attend);
            var passed = await query.CountAsync(al => al.Status == AttendanceStatus.Passed);
            var missed = await query.CountAsync(al => al.Status == AttendanceStatus.Missed);

            return new AttendanceSummaryResponseDto
            {
                TotalSessions = total,
                AttendedSessions = attended,
                PassedSessions = passed,
                MissedSessions = missed
            };
        }

        public async Task<double> GetCumulativeAttendancePercentageAsync(int studentId, string? sessionType)
        {
            var query = _attendanceRepo.GetStudentAttendanceWithSessionsQuery(studentId);

            if (!string.IsNullOrEmpty(sessionType))
            {
                if (Enum.TryParse<LiveSessionType>(sessionType, true, out var parsedType))
                {
                    query = query.Where(al => al.LiveSession.Type == parsedType);
                }
            }

            var total = await query.CountAsync();
            if (total == 0) return 0;

            var attended = await query.CountAsync(al =>
                al.Status == AttendanceStatus.Attend || al.Status == AttendanceStatus.Passed);

            return Math.Round((double)attended / total * 100, 2);
        }
    }
}
