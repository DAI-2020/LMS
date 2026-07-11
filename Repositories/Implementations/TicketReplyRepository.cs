using Microsoft.EntityFrameworkCore;
using LMS.API.Data;
using LMS.API.Models;
using LMS.API.Repositories.Interfaces;

namespace LMS.API.Repositories.Implementations
{
    public class TicketReplyRepository : ITicketReplyRepository
    {
        private readonly LMSDbContext _context;
        public TicketReplyRepository(LMSDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TicketReply>> GetByTicketIdAsync(int ticketId)
        {
            return await _context.TicketReplies
                .Include(tr => tr.User)
                .Where(tr => tr.TicketId == ticketId)
                .OrderBy(tr => tr.CreatedAt)
                .ToListAsync();
        }

        public async Task<TicketReply> AddAsync(TicketReply reply)
        {
            await _context.TicketReplies.AddAsync(reply);
            await _context.SaveChangesAsync();
            return reply;
        }

        public async Task DeleteAsync(int id)
        {
            var reply = await _context.TicketReplies.FindAsync(id);
            if (reply is not null)
            {
                _context.TicketReplies.Remove(reply);
                await _context.SaveChangesAsync();
            }
        }
    }
}
