using LMS.API.Data;
using LMS.API.Models;
using LMS.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS.API.Repositories.Implementations;

public class TopicRepository : Repository<Topic>, ITopicRepository
{
    public TopicRepository(LMSDbContext context)
        : base(context)
    {
    }

    public async Task<Topic?> GetByNameAsync(string name)
    {
        return await _dbSet
            .FirstOrDefaultAsync(x => x.Name == name);
    }
}
