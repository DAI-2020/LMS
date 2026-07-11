using LMS.API.Data;
using LMS.API.Models;
using LMS.API.Repositories.Interfaces;

namespace LMS.API.Repositories.Implementations
{
    public class MaterialRepository : Repository<Material>, IMaterialRepository
    {
        public MaterialRepository(LMSDbContext context)
      : base(context)
        {
        }

    }
}
