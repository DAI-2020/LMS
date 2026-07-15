using LMS.API.DTOs.Support;

namespace LMS.API.Services.Interfaces;

public interface ISupportAppService
{
    Task<SupportLandingDto> GetLandingInfoAsync(int userId);
    Task<List<TicketCategoryDto>> GetCategoriesAsync();
    Task<SystemStatusDto> GetSystemStatusAsync();
    Task<SupportTicketResponseDto?> SubmitTicketAsync(int userId, SubmitTicketRequestDto dto);
    Task<List<SupportTicketResponseDto>> GetMyTicketsAsync(int userId);
}
