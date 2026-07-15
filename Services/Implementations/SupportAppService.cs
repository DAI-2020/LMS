using LMS.API.DTOs.Support;
using LMS.API.Enums.Ticket;
using LMS.API.Models;
using LMS.API.Repositories.Interfaces;
using LMS.API.Services.Interfaces;

namespace LMS.API.Services.Implementations;

public class SupportAppService : ISupportAppService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITicketRepository _ticketRepo;
    private readonly IFileService _fileService;

    public SupportAppService(IUnitOfWork unitOfWork, ITicketRepository ticketRepo, IFileService fileService)
    {
        _unitOfWork = unitOfWork;
        _ticketRepo = ticketRepo;
        _fileService = fileService;
    }

    public async Task<SupportLandingDto> GetLandingInfoAsync(int userId)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        var firstName = user?.FullName?.Split(' ').FirstOrDefault() ?? "Student";

        return new SupportLandingDto
        {
            StudentFirstName = firstName,
            QuickActions = new List<QuickActionDto>
            {
                new() { IconKey = "submit-issue", Title = "Submit an Issue", Description = "Report a problem you're facing", ActionText = "Submit Now", RedirectUrl = "/support/submit" },
                new() { IconKey = "view-tickets", Title = "My Tickets", Description = "View your submitted tickets", ActionText = "View All", RedirectUrl = "/support/tickets" },
                new() { IconKey = "faqs", Title = "FAQs", Description = "Browse frequently asked questions", ActionText = "Browse", RedirectUrl = "/support/faqs" }
            },
            ContactChannels = new List<ContactChannelDto>
            {
                new() { ChannelType = "Hotline", ValueText = "+20 123 456 7890", ButtonText = "Call Now", ActionUrl = "tel:+201234567890" },
                new() { ChannelType = "Email", ValueText = "support@campus.com", ButtonText = "Send Email", ActionUrl = "mailto:support@campus.com" },
                new() { ChannelType = "WhatsApp", ValueText = "+20 123 456 7890", ButtonText = "Chat", ActionUrl = "https://wa.me/201234567890" }
            }
        };
    }

    public Task<List<TicketCategoryDto>> GetCategoriesAsync()
    {
        var categories = new List<TicketCategoryDto>
        {
            new() { Id = (int)TicketCategory.Technical, CategoryName = "Technical" },
            new() { Id = (int)TicketCategory.Academic, CategoryName = "Academic" },
            new() { Id = (int)TicketCategory.Financial, CategoryName = "Financial" }
        };
        return Task.FromResult(categories);
    }

    public Task<SystemStatusDto> GetSystemStatusAsync()
    {
        return Task.FromResult(new SystemStatusDto
        {
            IsOperational = true,
            StatusMessage = "All systems operational"
        });
    }

    public async Task<SupportTicketResponseDto?> SubmitTicketAsync(int userId, SubmitTicketRequestDto dto)
    {
        if (!Enum.TryParse<TicketCategory>(dto.CategoryId.ToString(), true, out var category))
        {
            throw new InvalidOperationException($"Invalid category ID: {dto.CategoryId}. Valid values are 0 (Technical), 1 (Academic), 2 (Financial).");
        }

        string? attachmentUrl = null;
        if (dto.Attachment is not null)
        {
            if (dto.Attachment.Length > 5 * 1024 * 1024)
                throw new InvalidOperationException("Attachment must not exceed 5MB.");

            var allowedExtensions = new[] { ".pdf", ".docx", ".doc", ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(dto.Attachment.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
                throw new InvalidOperationException($"File type '{extension}' is not allowed. Allowed types: {string.Join(", ", allowedExtensions)}.");

            attachmentUrl = await _fileService.UploadSubmissionAsync(dto.Attachment);
        }

        var ticket = new Ticket
        {
            StudentId = userId,
            Title = dto.Subject,
            Description = dto.Description,
            Category = category,
            Priority = TicketPriority.Medium,
            Status = TicketStatus.Open,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _ticketRepo.AddAsync(ticket);

        return new SupportTicketResponseDto
        {
            Id = created.Id,
            Subject = created.Title,
            CategoryName = created.Category.ToString(),
            Description = created.Description,
            Status = created.Status.ToString(),
            CreatedAt = created.CreatedAt,
            AttachmentUrl = attachmentUrl
        };
    }

    public async Task<List<SupportTicketResponseDto>> GetMyTicketsAsync(int userId)
    {
        var tickets = await _ticketRepo.GetFilteredAsync(null, null, null, userId);
        return tickets.Select(t => new SupportTicketResponseDto
        {
            Id = t.Id,
            Subject = t.Title,
            CategoryName = t.Category.ToString(),
            Description = t.Description,
            Status = t.Status.ToString(),
            CreatedAt = t.CreatedAt
        }).ToList();
    }
}
