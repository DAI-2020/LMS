using LMS.API.DTOs.LiveSession;
using LMS.API.Enums.LiveSession;
using LMS.API.Models;
using LMS.API.Repositories.Interfaces;
using LMS.API.Services.Interfaces;

namespace LMS.API.Services.Implementations
{
    public class LiveSessionService : ILiveSessionService
    {
        private readonly ILiveSessionRepository _repository;

        public LiveSessionService(ILiveSessionRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<LiveSessionResponseDto>> GetAllAsync()
        {
            var sessions = await _repository.GetAllAsync();
            return sessions.Select(MapToDto);
        }

        public async Task<LiveSessionResponseDto?> GetByIdAsync(int id)
        {
            var session = await _repository.GetByIdAsync(id);
            return session is null ? null : MapToDto(session);
        }

        public async Task<IEnumerable<LiveSessionResponseDto>> GetFilteredAsync(
            string? status, string? type, string? mode, int? courseId, DateTime? from, DateTime? to)
        {
            LiveSessionStatus? parsedStatus = null;
            LiveSessionType? parsedType = null;
            DeliveryMode? parsedMode = null;

            if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<LiveSessionStatus>(status, true, out var s))
                parsedStatus = s;
            if (!string.IsNullOrWhiteSpace(type) && Enum.TryParse<LiveSessionType>(type, true, out var t))
                parsedType = t;
            if (!string.IsNullOrWhiteSpace(mode) && Enum.TryParse<DeliveryMode>(mode, true, out var m))
                parsedMode = m;

            var sessions = await _repository.GetFilteredAsync(parsedStatus, parsedType, parsedMode, courseId, from, to);
            return sessions.Select(MapToDto);
        }

        public async Task<IEnumerable<LiveSessionResponseDto>> GetTodaySessionsAsync()
        {
            var today = DateTime.Today;
            var sessions = await _repository.GetFilteredAsync(null, null, null, null, today, today.AddDays(1).AddTicks(-1));
            return sessions.Select(MapToDto);
        }

        public async Task<IEnumerable<LiveSessionResponseDto>> GetUpcomingSessionsAsync()
        {
            var sessions = await _repository.GetUpcomingSessionsAsync();
            return sessions.Select(MapToDto);
        }

        public async Task<IEnumerable<LiveSessionResponseDto>> GetRecordedSessionsAsync()
        {
            var all = await _repository.GetAllAsync();
            var recorded = all.Where(ls => !string.IsNullOrEmpty(ls.RecordingUrl));
            return recorded.Select(MapToDto);
        }

        public async Task<IEnumerable<LiveSessionResponseDto>> GetMissedSessionsAsync()
        {
            var all = await _repository.GetAllAsync();
            var now = DateTime.UtcNow;
            var missed = all.Where(ls =>
                ls.Status == LiveSessionStatus.Scheduled &&
                ls.ScheduledAt.AddMinutes(ls.DurationMinutes) < now);
            return missed.Select(MapToDto);
        }

        public async Task<IEnumerable<LiveSessionResponseDto>> GetCompletedSessionsAsync()
        {
            var sessions = await _repository.GetFilteredAsync(LiveSessionStatus.Completed, null, null, null, null, null);
            return sessions.Select(MapToDto);
        }

        public async Task<LiveSessionResponseDto> CreateAsync(CreateLiveSessionDto dto)
        {
            if (!Enum.TryParse<LiveSessionType>(dto.Type, true, out var type))
                throw new ArgumentException($"Invalid LiveSessionType: {dto.Type}");
            if (!Enum.TryParse<DeliveryMode>(dto.Mode, true, out var mode))
                throw new ArgumentException($"Invalid DeliveryMode: {dto.Mode}");

            var session = new LiveSession
            {
                CourseId = dto.CourseId,
                Title = dto.Title,
                ScheduledAt = dto.ScheduledAt,
                DurationMinutes = dto.DurationMinutes,
                Status = LiveSessionStatus.Scheduled,
                Type = type,
                Mode = mode
            };

            var created = await _repository.AddAsync(session);
            return MapToDto(created);
        }

        public async Task<LiveSessionResponseDto?> UpdateAsync(int id, UpdateLiveSessionDto dto)
        {
            var session = await _repository.GetByIdAsync(id);
            if (session is null) return null;

            if (dto.CourseId.HasValue) session.CourseId = dto.CourseId.Value;
            if (dto.Title is not null) session.Title = dto.Title;
            if (dto.ScheduledAt.HasValue) session.ScheduledAt = dto.ScheduledAt.Value;
            if (dto.DurationMinutes.HasValue) session.DurationMinutes = dto.DurationMinutes.Value;
            if (dto.WeekNumber.HasValue) session.WeekNumber = dto.WeekNumber.Value;
            if (dto.RecordingUrl is not null) session.RecordingUrl = dto.RecordingUrl;

            if (!string.IsNullOrWhiteSpace(dto.Status) && Enum.TryParse<LiveSessionStatus>(dto.Status, true, out var status))
                session.Status = status;
            if (!string.IsNullOrWhiteSpace(dto.Type) && Enum.TryParse<LiveSessionType>(dto.Type, true, out var type))
                session.Type = type;
            if (!string.IsNullOrWhiteSpace(dto.Mode) && Enum.TryParse<DeliveryMode>(dto.Mode, true, out var mode))
                session.Mode = mode;

            await _repository.UpdateAsync(session);
            return MapToDto(session);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }

        public async Task<SessionTimelineResponseDto?> GetTimelineAsync(SessionTimelineFilterDto filter)
        {
            var year = filter.Year ?? DateTime.UtcNow.Year;
            var month = filter.Month ?? DateTime.UtcNow.Month;
            var startDate = new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Utc);
            var endDate = startDate.AddMonths(1).AddTicks(-1);

            LiveSessionStatus? parsedStatus = null;
            DeliveryMode? parsedMode = null;

            if (!string.IsNullOrWhiteSpace(filter.Category))
            {
                if (filter.Category.Equals("Completed", StringComparison.OrdinalIgnoreCase))
                    parsedStatus = LiveSessionStatus.Completed;
                else if (filter.Category.Equals("Missed", StringComparison.OrdinalIgnoreCase))
                    parsedStatus = LiveSessionStatus.Scheduled;
            }

            if (!string.IsNullOrWhiteSpace(filter.Mode) && Enum.TryParse<DeliveryMode>(filter.Mode, true, out var m))
                parsedMode = m;

            var sessions = await _repository.GetFilteredAsync(parsedStatus, null, parsedMode, filter.CourseId, startDate, endDate);

            if (!string.IsNullOrWhiteSpace(filter.Category) &&
                filter.Category.Equals("Missed", StringComparison.OrdinalIgnoreCase))
            {
                var now = DateTime.UtcNow;
                sessions = sessions.Where(s =>
                    s.Status == LiveSessionStatus.Scheduled &&
                    s.ScheduledAt.AddMinutes(s.DurationMinutes) < now);
            }

            if (!string.IsNullOrWhiteSpace(filter.Category) &&
                filter.Category.Equals("TodaysSession", StringComparison.OrdinalIgnoreCase))
            {
                sessions = sessions.Where(s => s.ScheduledAt.Date == DateTime.UtcNow.Date);
            }

            if (filter.WeekNumber.HasValue)
                sessions = sessions.Where(s => s.WeekNumber == filter.WeekNumber.Value);

            var grouped = sessions
                .GroupBy(s => s.ScheduledAt.Date)
                .OrderBy(g => g.Key);

            var days = grouped.Select(g => new SessionTimelineDayDto
            {
                Date = g.Key,
                DayName = g.Key.ToString("dddd"),
                SessionCount = g.Count(),
                Sessions = g.Select(MapToDto).ToList()
            }).ToList();

            return new SessionTimelineResponseDto
            {
                Year = year,
                Month = month,
                MonthName = startDate.ToString("MMMM"),
                Days = days,
                TotalSessions = sessions.Count()
            };
        }

        public async Task<PaginatedSessionsResponseDto> GetPaginatedAsync(PaginatedSessionsRequestDto request)
        {
            var page = Math.Max(1, request.Page);
            var pageSize = Math.Clamp(request.PageSize, 1, 100);

            DeliveryMode? parsedMode = null;
            LiveSessionStatus? parsedStatus = null;
            LiveSessionType? parsedType = null;

            if (!string.IsNullOrWhiteSpace(request.Mode) && Enum.TryParse<DeliveryMode>(request.Mode, true, out var mode))
                parsedMode = mode;
            if (!string.IsNullOrWhiteSpace(request.Status) && Enum.TryParse<LiveSessionStatus>(request.Status, true, out var status))
                parsedStatus = status;
            if (!string.IsNullOrWhiteSpace(request.Type) && Enum.TryParse<LiveSessionType>(request.Type, true, out var type))
                parsedType = type;

            var allSessions = await _repository.GetFilteredAsync(parsedStatus, parsedType, parsedMode, request.CourseId, null, null);

            if (!string.IsNullOrWhiteSpace(request.Search))
                allSessions = allSessions.Where(s =>
                    s.Title.Contains(request.Search, StringComparison.OrdinalIgnoreCase) ||
                    (s.Course != null && s.Course.Title != null && s.Course.Title.Contains(request.Search, StringComparison.OrdinalIgnoreCase)));

            var list = allSessions.ToList();
            var totalCount = list.Count;

            var paged = list
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginatedSessionsResponseDto
            {
                Items = paged.Select(MapToCardDto).ToList(),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        private static LiveSessionResponseDto MapToDto(LiveSession session)
        {
            var now = DateTime.UtcNow;
            var sessionEnd = session.ScheduledAt.AddMinutes(session.DurationMinutes);
            string actionLabel;
            if (session.Status == LiveSessionStatus.Completed || (now > sessionEnd && session.Status != LiveSessionStatus.Live))
                actionLabel = "View Session Record";
            else
                actionLabel = "Join Session";

            return new LiveSessionResponseDto
            {
                Id = session.Id,
                CourseId = session.CourseId,
                CourseName = session.Course?.Title ?? string.Empty,
                Title = session.Title,
                WeekNumber = session.WeekNumber,
                ScheduledAt = session.ScheduledAt,
                DurationMinutes = session.DurationMinutes,
                Status = session.Status.ToString(),
                Type = session.Type.ToString(),
                Mode = session.Mode.ToString(),
                RecordingUrl = session.RecordingUrl,
                AttendanceCount = session.AttendanceLogs?.Count ?? 0,
                HasAttendance = session.AttendanceLogs?.Any() ?? false,
                HasTask = session.Materials?.Any() ?? false,
                HasSurvey = false,
                ActionLabel = actionLabel
            };
        }

        private static LiveSessionCardDto MapToCardDto(LiveSession session)
        {
            var now = DateTime.UtcNow;
            var sessionEnd = session.ScheduledAt.AddMinutes(session.DurationMinutes);
            string actionLabel;
            if (session.Status == LiveSessionStatus.Completed || (now > sessionEnd && session.Status != LiveSessionStatus.Live))
                actionLabel = "View Session Record";
            else
                actionLabel = "Join Session";

            return new LiveSessionCardDto
            {
                Id = session.Id,
                Title = session.Title,
                CourseName = session.Course?.Title ?? string.Empty,
                Status = session.Status.ToString(),
                Type = session.Type.ToString(),
                Mode = session.Mode.ToString(),
                ScheduledAt = session.ScheduledAt,
                DurationMinutes = session.DurationMinutes,
                HasAttendance = session.AttendanceLogs?.Any() ?? false,
                HasTask = session.Materials?.Any() ?? false,
                HasSurvey = false,
                RecordingUrl = session.RecordingUrl,
                ActionLabel = actionLabel,
                AttendanceCount = session.AttendanceLogs?.Count ?? 0
            };
        }
    }
}
