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
                Status = LiveSessionStatus.Upcoming,
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

            if (dto.Title is not null) session.Title = dto.Title;
            if (dto.ScheduledAt.HasValue) session.ScheduledAt = dto.ScheduledAt.Value;
            if (dto.DurationMinutes.HasValue) session.DurationMinutes = dto.DurationMinutes.Value;
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

        private static LiveSessionResponseDto MapToDto(LiveSession session)
        {
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
                AttendanceCount = session.AttendanceLogs?.Count ?? 0
            };
        }
    }
}
