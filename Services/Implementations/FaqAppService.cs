using LMS.API.DTOs.FAQs;
using LMS.API.Repositories.Interfaces;
using LMS.API.Services.Interfaces;

namespace LMS.API.Services.Implementations;

public class FaqAppService : IFaqAppService
{
    private readonly IFaqRepository _faqRepository;

    public FaqAppService(IFaqRepository faqRepository)
    {
        _faqRepository = faqRepository;
    }

    public async Task<FaqPageResponseDto> GetFaqPageAsync()
    {
        var faqs = await _faqRepository.GetAllAsync();

        return new FaqPageResponseDto
        {
            Title = "Frequently Asked Questions",
            Subtitle = "Find answers to common questions",
            Faqs = faqs.Select((f, index) => new FaqItemDto
            {
                Id = f.Id,
                Order = index + 1,
                Question = f.Question,
                Answer = f.Answer
            }).ToList()
        };
    }
}
