using LMS.API.Data;
using LMS.API.Models;
using LMS.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS.API.Repositories.Implementations;

public class TopicRepository: Repository<Topic>,ITopicRepository
{
    private readonly LMSDbContext _context;

    public TopicRepository(LMSDbContext context)
        : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Topic>> GetTopicsByCourseAsync(int courseId)
    {
        return await _context.Topics
            .Where(x => x.CourseId == courseId)
            .ToListAsync();
    }
}