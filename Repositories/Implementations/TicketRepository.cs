using Microsoft.EntityFrameworkCore;
using LMS.API.Data;
using LMS.API.Enums.Ticket;
using LMS.API.Models;
using LMS.API.Repositories.Interfaces;

namespace LMS.API.Repositories.Implementations
{
    public class TicketRepository : ITicketRepository
    {
        private readonly LMSDbContext _context;

        public TicketRepository(LMSDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ticket>> GetAllAsync()
        {
            return await _context.Tickets
                .Include(t => t.Student)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<Ticket?> GetByIdAsync(int id)
        {
            return await _context.Tickets
                .Include(t => t.Student)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Ticket?> GetByIdWithRepliesAsync(int id)
        {
            return await _context.Tickets
                .Include(t => t.Student)
                .Include(t => t.TicketReplies)
                    .ThenInclude(tr => tr.User)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Ticket>> GetFilteredAsync(
            TicketStatus? status, TicketCategory? category, TicketPriority? priority, int? studentId)
        {
            var query = _context.Tickets
                .Include(t => t.Student)
                .AsQueryable();

            if (status.HasValue)
                query = query.Where(t => t.Status == status.Value);
            if (category.HasValue)
                query = query.Where(t => t.Category == category.Value);
            if (priority.HasValue)
                query = query.Where(t => t.Priority == priority.Value);
            if (studentId.HasValue)
                query = query.Where(t => t.StudentId == studentId.Value);

            return await query.OrderByDescending(t => t.CreatedAt).ToListAsync();
        }

        public async Task<Ticket> AddAsync(Ticket ticket)
        {
            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }

        public async Task UpdateAsync(Ticket ticket)
        {
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket is not null)
            {
                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
            }
        }
    }
}
