using LMS.API.DTOs.LiveSession;

namespace LMS.API.Services.Interfaces
{
    public interface ILiveSessionService
    {
        Task<IEnumerable<LiveSessionResponseDto>> GetAllAsync();
        Task<LiveSessionResponseDto?> GetByIdAsync(int id);
        Task<IEnumerable<LiveSessionResponseDto>> GetFilteredAsync(string? status, string? type, string? mode, int? courseId, DateTime? from, DateTime? to);
        Task<IEnumerable<LiveSessionResponseDto>> GetTodaySessionsAsync();
        Task<IEnumerable<LiveSessionResponseDto>> GetUpcomingSessionsAsync();
        Task<IEnumerable<LiveSessionResponseDto>> GetRecordedSessionsAsync();
        Task<IEnumerable<LiveSessionResponseDto>> GetMissedSessionsAsync();
        Task<IEnumerable<LiveSessionResponseDto>> GetCompletedSessionsAsync();
        Task<LiveSessionResponseDto> CreateAsync(CreateLiveSessionDto dto);
        Task<LiveSessionResponseDto?> UpdateAsync(int id, UpdateLiveSessionDto dto);
        Task<bool> DeleteAsync(int id);
        Task<SessionTimelineResponseDto?> GetTimelineAsync(SessionTimelineFilterDto filter);
        Task<PaginatedSessionsResponseDto> GetPaginatedAsync(PaginatedSessionsRequestDto request);
    }
}
