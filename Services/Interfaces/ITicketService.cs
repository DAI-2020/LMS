using LMS.API.DTOs.Ticket;

namespace LMS.API.Services.Interfaces
{
    public interface ITicketService
    {
        Task<IEnumerable<TicketResponseDto>> GetAllAsync();
        Task<TicketResponseDto?> GetByIdAsync(int id);
        Task<IEnumerable<TicketResponseDto>> GetFilteredAsync(string? status, string? category, string? priority, int? studentId);
        Task<TicketResponseDto> CreateAsync(CreateTicketDto dto);
        Task<TicketReplyDto> AddReplyAsync(int ticketId, int userId, string message);
        Task<TicketResponseDto?> UpdateStatusAsync(int ticketId, string status);
        Task<bool> DeleteAsync(int id);
    }
}
