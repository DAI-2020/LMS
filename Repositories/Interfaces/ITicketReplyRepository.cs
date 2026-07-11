using LMS.API.Models;

namespace LMS.API.Repositories.Interfaces
{
    public interface ITicketReplyRepository
    {
        Task<IEnumerable<TicketReply>> GetByTicketIdAsync(int ticketId);
        Task<TicketReply> AddAsync(TicketReply reply);
        Task DeleteAsync(int id);
    }
}
