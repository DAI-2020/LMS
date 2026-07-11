using LMS.API.Enums.LiveSession;
using LMS.API.Models;

namespace LMS.API.Repositories.Interfaces
{
    public interface ILiveSessionRepository
    {
        Task<IEnumerable<LiveSession>> GetAllAsync();
        Task<LiveSession?> GetByIdAsync(int id);
        Task<IEnumerable<LiveSession>> GetFilteredAsync(LiveSessionStatus? status, LiveSessionType? type, DeliveryMode? mode, int? courseId, DateTime? from, DateTime? to);
        Task<IEnumerable<LiveSession>> GetUpcomingSessionsAsync();
        Task<LiveSession> AddAsync(LiveSession session);
        Task UpdateAsync(LiveSession session);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
