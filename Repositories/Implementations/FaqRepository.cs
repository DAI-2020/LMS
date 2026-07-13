using LMS.API.Data;
using LMS.API.Models;
using LMS.API.Repositories.Interfaces;

namespace LMS.API.Repositories.Implementations
{
    public class FaqRepository : Repository<FAQ>, IFaqRepository
    {
        public FaqRepository(LMSDbContext context)
            : base(context)
        {
        }
    }
}
