using LMS.API.Enums.Ticket;
using LMS.API.Models;

namespace LMS.API.Repositories.Interfaces
{
    public interface ITicketRepository
    {
        Task<IEnumerable<Ticket>> GetAllAsync();
        Task<Ticket?> GetByIdAsync(int id);
        Task<Ticket?> GetByIdWithRepliesAsync(int id);
        Task<IEnumerable<Ticket>> GetFilteredAsync(TicketStatus? status, TicketCategory? category, TicketPriority? priority, int? studentId);
        Task<Ticket> AddAsync(Ticket ticket);
        Task UpdateAsync(Ticket ticket);
        Task DeleteAsync(int id);
    }
}
