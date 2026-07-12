using LMS.API.Models;

namespace LMS.API.Repositories.Interfaces
{
    public interface IFaqRepository
    {
        Task<IEnumerable<FAQ>> GetAllAsync();
        Task<FAQ> GetByIdAsync(int id);
        Task AddAsync(FAQ faq);
        void Update(FAQ faq);
        void Delete(FAQ faq);
    }
}
