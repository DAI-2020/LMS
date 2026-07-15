using LMS.API.DTOs.FAQs;

namespace LMS.API.Services.Interfaces;

public interface IFaqAppService
{
    Task<FaqPageResponseDto> GetFaqPageAsync();
}
