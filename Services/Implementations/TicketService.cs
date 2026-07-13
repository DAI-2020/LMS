using LMS.API.DTOs.Ticket;
using LMS.API.Enums.Ticket;
using LMS.API.Models;
using LMS.API.Repositories.Interfaces;
using LMS.API.Services.Interfaces;

namespace LMS.API.Services.Implementations
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepo;
        private readonly ITicketReplyRepository _replyRepo;

        public TicketService(ITicketRepository ticketRepo, ITicketReplyRepository replyRepo)
        {
            _ticketRepo = ticketRepo;
            _replyRepo = replyRepo;
        }

        public async Task<IEnumerable<TicketResponseDto>> GetAllAsync()
        {
            var tickets = await _ticketRepo.GetAllAsync();
            return tickets.Select(MapToDto);
        }

        public async Task<TicketResponseDto?> GetByIdAsync(int id)
        {
            var ticket = await _ticketRepo.GetByIdWithRepliesAsync(id);
            return ticket is null ? null : MapToDto(ticket);
        }

        public async Task<IEnumerable<TicketResponseDto>> GetMyTicketsAsync(int studentId)
        {
            var tickets = await _ticketRepo.GetFilteredAsync(null, null, null, studentId);
            return tickets.Select(MapToDto);
        }

        public async Task<IEnumerable<TicketResponseDto>> GetFilteredAsync(
            string? status, string? category, string? priority, int? studentId)
        {
            TicketStatus? parsedStatus = null;
            TicketCategory? parsedCategory = null;
            TicketPriority? parsedPriority = null;

            if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<TicketStatus>(status, true, out var s))
                parsedStatus = s;
            if (!string.IsNullOrWhiteSpace(category) && Enum.TryParse<TicketCategory>(category, true, out var c))
                parsedCategory = c;
            if (!string.IsNullOrWhiteSpace(priority) && Enum.TryParse<TicketPriority>(priority, true, out var p))
                parsedPriority = p;

            var tickets = await _ticketRepo.GetFilteredAsync(parsedStatus, parsedCategory, parsedPriority, studentId);
            return tickets.Select(MapToDto);
        }

        public async Task<TicketResponseDto> CreateAsync(CreateTicketDto dto)
        {
            if (!Enum.TryParse<TicketCategory>(dto.Category, true, out var category))
                throw new ArgumentException($"Invalid TicketCategory: {dto.Category}");
            if (!Enum.TryParse<TicketPriority>(dto.Priority, true, out var priority))
                throw new ArgumentException($"Invalid TicketPriority: {dto.Priority}");

            var ticket = new Ticket
            {
                StudentId = dto.StudentId,
                Title = dto.Title,
                Description = dto.Description,
                Category = category,
                Priority = priority,
                Status = TicketStatus.Open,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _ticketRepo.AddAsync(ticket);
            return MapToDto(created);
        }

        public async Task<TicketReplyDto> AddReplyAsync(int ticketId, int userId, string message)
        {
            var ticket = await _ticketRepo.GetByIdAsync(ticketId);
            if (ticket is null)
                throw new KeyNotFoundException($"Ticket {ticketId} not found");

            var reply = new TicketReply
            {
                TicketId = ticketId,
                UserId = userId,
                Message = message,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _replyRepo.AddAsync(reply);

            if (ticket.Status == TicketStatus.Open)
            {
                ticket.Status = TicketStatus.InProgress;
                ticket.UpdatedAt = DateTime.UtcNow;
                await _ticketRepo.UpdateAsync(ticket);
            }

            return new TicketReplyDto
            {
                Id = created.Id,
                UserId = created.UserId,
                UserName = created.User?.FullName ?? string.Empty,
                Message = created.Message,
                CreatedAt = created.CreatedAt
            };
        }

        public async Task<TicketResponseDto?> UpdateStatusAsync(int ticketId, string status)
        {
            if (!Enum.TryParse<TicketStatus>(status, true, out var parsedStatus))
                throw new ArgumentException($"Invalid TicketStatus: {status}");

            var ticket = await _ticketRepo.GetByIdWithRepliesAsync(ticketId);
            if (ticket is null) return null;

            ticket.Status = parsedStatus;
            ticket.UpdatedAt = DateTime.UtcNow;
            await _ticketRepo.UpdateAsync(ticket);

            return MapToDto(ticket);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var ticket = await _ticketRepo.GetByIdAsync(id);
            if (ticket is null) return false;
            await _ticketRepo.DeleteAsync(id);
            return true;
        }

        private static TicketResponseDto MapToDto(Ticket ticket)
        {
            return new TicketResponseDto
            {
                Id = ticket.Id,
                StudentId = ticket.StudentId,
                StudentName = ticket.Student?.FullName ?? string.Empty,
                Title = ticket.Title,
                Description = ticket.Description,
                Category = ticket.Category.ToString(),
                Priority = ticket.Priority.ToString(),
                Status = ticket.Status.ToString(),
                CreatedAt = ticket.CreatedAt,
                UpdatedAt = ticket.UpdatedAt,
                Replies = ticket.TicketReplies?.Select(r => new TicketReplyDto
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    UserName = r.User?.FullName ?? string.Empty,
                    Message = r.Message,
                    CreatedAt = r.CreatedAt
                }).ToList() ?? new List<TicketReplyDto>()
            };
        }
    }
}
