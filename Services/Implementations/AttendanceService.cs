using LMS.API.DTOs.Attendance;
using LMS.API.Enums.AttendanceLog;
using LMS.API.Enums.LiveSession;
using LMS.API.Models;
using LMS.API.Repositories.Implementations;
using LMS.API.Repositories.Interfaces;
using LMS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS.API.Services.Implementations
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceLogRepository _attendanceRepo;
        private readonly ILiveSessionRepository _sessionRepo;

        private const double PassThreshold = 0.7;

        public AttendanceService(IAttendanceLogRepository attendanceRepo, ILiveSessionRepository sessionRepo)
        {
            _attendanceRepo = attendanceRepo;
            _sessionRepo = sessionRepo;
        }

        public async Task<bool> JoinSessionAsync(JoinSessionDto dto)
        {
            var existing = await _attendanceRepo.GetAttendanceLogAsync(dto.SessionId, dto.StudentId);
            if (existing is not null) return false;

            var log = new AttendanceLog
            {
                SessionId = dto.SessionId,
                StudentId = dto.StudentId,
                JoinTime = DateTime.UtcNow,
                Status = AttendanceStatus.Attend
            };

            await _attendanceRepo.AddAsync(log);
            return true;
        }

        public async Task<bool> LeaveSessionAsync(LeaveSessionDto dto)
        {
            var log = await _attendanceRepo.GetAttendanceLogAsync(dto.SessionId, dto.StudentId);
            if (log is null || log.LeaveTime is not null) return false;

            var session = await _sessionRepo.GetByIdAsync(dto.SessionId);
            if (session is null) return false;

            log.LeaveTime = DateTime.UtcNow;
            log.MicrophoneUsageSeconds = dto.MicrophoneUsageSeconds;

            var timeSpentMinutes = (log.LeaveTime.Value - log.JoinTime!.Value).TotalMinutes;
            var durationMinutes = session.DurationMinutes;

            var engagementScore = CalculateEngagementScore(timeSpentMinutes, durationMinutes, dto.MicrophoneUsageSeconds);
            log.EngagementScore = engagementScore;
            log.ParticipationLevel = CalculateParticipationLevel(engagementScore);

            if (timeSpentMinutes / durationMinutes >= PassThreshold)
                log.Status = AttendanceStatus.Passed;

            await _attendanceRepo.UpdateAsync(log);
            return true;
        }

        public async Task<StudentAttendanceSummaryDto?> GetStudentSummaryAsync(int studentId)
        {
            var summary = await _attendanceRepo.GetStudentSummaryAsync(studentId);
            if (summary is null) return null;

            var studentLogs = await _attendanceRepo.GetByStudentIdAsync(studentId);
            var firstLog = studentLogs.FirstOrDefault();
            summary.StudentName = firstLog?.Student?.FullName ?? string.Empty;

            return summary;
        }

        public async Task<IEnumerable<AttendanceLogDto>> GetSessionAttendanceAsync(int sessionId)
        {
            var logs = await _attendanceRepo.GetBySessionIdAsync(sessionId);
            return logs.Select(l => new AttendanceLogDto
            {
                Id = l.Id,
                SessionId = l.SessionId,
                SessionTitle = l.LiveSession?.Title ?? string.Empty,
                StudentId = l.StudentId,
                StudentName = l.Student?.FullName ?? string.Empty,
                JoinTime = l.JoinTime,
                LeaveTime = l.LeaveTime,
                Status = l.Status.ToString(),
                ParticipationLevel = l.ParticipationLevel.ToString(),
                EngagementScore = l.EngagementScore,
                MicrophoneUsageSeconds = l.MicrophoneUsageSeconds
            });
        }

        private static int CalculateEngagementScore(double timeSpentMinutes, double durationMinutes, int micSeconds)
        {
            var timeFactor = Math.Min(timeSpentMinutes / durationMinutes, 1.0) * 70;
            var micFactor = durationMinutes > 0
                ? Math.Min(micSeconds / (durationMinutes * 60), 1.0) * 30
                : 0;

            return (int)Math.Round(Math.Min(timeFactor + micFactor, 100));
        }

        private static ParticipationLevel CalculateParticipationLevel(int score)
        {
            if (score >= 67) return ParticipationLevel.Excellent;
            if (score >= 34) return ParticipationLevel.Good;
            return ParticipationLevel.Weak;
        }

        public async Task<AttendanceSummaryResponseDto> GetAttendanceSummaryAsync(int studentId, AttendanceFilterDto filter)
        {
            var query = _attendanceRepo.GetStudentAttendanceWithSessionsQuery(studentId);

            //(Technical / Non-Technical)
            if (!string.IsNullOrEmpty(filter.SessionType))
            {
                if (Enum.TryParse<LiveSessionType>(filter.SessionType, true, out var parsedType))
                {
                    query = query.Where(al => al.LiveSession.Type == parsedType);
                }
            }

            //(This Month / This Year)
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

            //CountAsync
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
    }
}
