using LMS.API.Data;
using LMS.API.Models;
using LMS.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS.API.Repositories.Implementations
{
    public class FaqRepository : IFaqRepository
    {
        private readonly LMSDbContext _context;

        public FaqRepository(LMSDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FAQ>> GetAllAsync()
        {
            return await _context.FAQs.ToListAsync();
        }

        public async Task<FAQ> GetByIdAsync(int id)
        {
            return await _context.FAQs.FindAsync(id);
        }

        public async Task AddAsync(FAQ faq)
        {
            await _context.FAQs.AddAsync(faq);
        }

        public void Update(FAQ faq)
        {
            _context.FAQs.Update(faq);
        }

        public void Delete(FAQ faq)
        {
            _context.FAQs.Remove(faq);
        }
    }
}
