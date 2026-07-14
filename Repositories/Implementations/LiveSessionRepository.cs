using Microsoft.EntityFrameworkCore;
using LMS.API.Data;
using LMS.API.Enums.LiveSession;
using LMS.API.Models;
using LMS.API.Repositories.Interfaces;

namespace LMS.API.Repositories.Implementations
{
    public class LiveSessionRepository : ILiveSessionRepository
    {
        private readonly LMSDbContext _context;
        public LiveSessionRepository(LMSDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LiveSession>> GetAllAsync()
        {
            return await _context.LiveSessions
                .Include(ls => ls.Course)
                .Include(ls => ls.AttendanceLogs)
                .Include(ls => ls.Materials)
                .OrderBy(ls => ls.WeekNumber)
                .ThenByDescending(ls => ls.ScheduledAt)
                .ToListAsync();
        }

        public async Task<LiveSession?> GetByIdAsync(int id)
        {
            return await _context.LiveSessions
                .Include(ls => ls.Course)
                .Include(ls => ls.AttendanceLogs)
                .Include(ls => ls.Materials)
                .FirstOrDefaultAsync(ls => ls.Id == id);
        }

        public async Task<IEnumerable<LiveSession>> GetFilteredAsync(
            LiveSessionStatus? status, LiveSessionType? type, DeliveryMode? mode,
            int? courseId, DateTime? from, DateTime? to)
        {
            var query = _context.LiveSessions
                .Include(ls => ls.Course)
                .Include(ls => ls.AttendanceLogs)
                .Include(ls => ls.Materials)
                .AsQueryable();

            if (status.HasValue)
                query = query.Where(ls => ls.Status == status.Value);
            if (type.HasValue)
                query = query.Where(ls => ls.Type == type.Value);
            if (mode.HasValue)
                query = query.Where(ls => ls.Mode == mode.Value);
            if (courseId.HasValue)
                query = query.Where(ls => ls.CourseId == courseId.Value);
            if (from.HasValue)
                query = query.Where(ls => ls.ScheduledAt >= from.Value);
            if (to.HasValue)
                query = query.Where(ls => ls.ScheduledAt <= to.Value);

            return await query
                .OrderBy(ls => ls.WeekNumber)
                .ThenByDescending(ls => ls.ScheduledAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<LiveSession>> GetUpcomingSessionsAsync()
        {
            return await _context.LiveSessions
                .Include(ls => ls.Course)
                .Include(ls => ls.AttendanceLogs)
                .Include(ls => ls.Materials)
                .Where(ls => ls.Status == LiveSessionStatus.Upcoming || ls.Status == LiveSessionStatus.Live)
                .OrderBy(ls => ls.WeekNumber)        
                .ThenBy(ls => ls.ScheduledAt)
                .ToListAsync();
        }

        public async Task<LiveSession> AddAsync(LiveSession session)
        {
            await _context.LiveSessions.AddAsync(session);
            await _context.SaveChangesAsync();
            return session;
        }

        public async Task UpdateAsync(LiveSession session)
        {
            _context.LiveSessions.Update(session);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var session = await _context.LiveSessions.FindAsync(id);
            if (session is not null)
            {
                _context.LiveSessions.Remove(session);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.LiveSessions.AnyAsync(ls => ls.Id == id);
        }

        public async Task<int> CountAsync(Func<LiveSession, bool> predicate)
        {
            return await Task.FromResult(_context.LiveSessions
                .Include(ls => ls.Course)
                .Where(predicate)
                .Count());
        }
    }
}
