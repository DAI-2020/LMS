using LMS.API.DTOs.FAQs;

namespace LMS.API.Services.Interfaces
{
    public interface IFaqService
    {
        Task<IEnumerable<FaqDto>> GetAllFaqsAsync();
        Task<FaqDto> GetFaqByIdAsync(int id);
        Task<FaqDto> CreateFaqAsync(UpsertFaqDto dto);
        Task<bool> UpdateFaqAsync(int id, UpsertFaqDto dto);
        Task<bool> DeleteFaqAsync(int id);
    }
}
