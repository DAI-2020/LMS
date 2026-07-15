using LMS.API.DTOs.FAQs;
using LMS.API.Models;
using LMS.API.Repositories.Interfaces;
using LMS.API.Services.Interfaces;

namespace LMS.API.Services.Implementations
{
    public class FaqService : IFaqService
    {
        private readonly IFaqRepository _faqRepository;
        private readonly IUnitOfWork _unitOfWork;

        public FaqService(IFaqRepository faqRepository, IUnitOfWork unitOfWork)
        {
            _faqRepository = faqRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<FaqDto>> GetAllFaqsAsync()
        {
            var faqs = await _faqRepository.GetAllAsync();
            return faqs.Select(f => new FaqDto
            {
                Id = f.Id,
                Question = f.Question,
                Answer = f.Answer
            });
        }

        public async Task<FaqDto?> GetFaqByIdAsync(int id)
        {
            var faq = await _faqRepository.GetByIdAsync(id);
            if (faq == null) return null;

            return new FaqDto
            {
                Id = faq.Id,
                Question = faq.Question,
                Answer = faq.Answer
            };
        }

        public async Task<FaqDto> CreateFaqAsync(UpsertFaqDto dto)
        {
            var faq = new FAQ
            {
                Question = dto.Question,
                Answer = dto.Answer
            };

            await _faqRepository.AddAsync(faq);
            await _unitOfWork.SaveChangesAsync();

            return new FaqDto
            {
                Id = faq.Id,
                Question = faq.Question,
                Answer = faq.Answer
            };
        }

        public async Task<bool> UpdateFaqAsync(int id, UpsertFaqDto dto)
        {
            var faq = await _faqRepository.GetByIdAsync(id);
            if (faq == null) return false;

            faq.Question = dto.Question;
            faq.Answer = dto.Answer;

            _faqRepository.Update(faq);
            var result = await _unitOfWork.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> DeleteFaqAsync(int id)
        {
            var faq = await _faqRepository.GetByIdAsync(id);
            if (faq == null) return false;

            _faqRepository.Delete(faq);
            var result = await _unitOfWork.SaveChangesAsync();

            return result > 0;
        }
    }
}
