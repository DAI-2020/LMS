using LMS.API.Data;
using LMS.API.Models;
using LMS.API.Repositories.Interfaces;

namespace LMS.API.Repositories.Implementations;

public class CourseTaskRepository : Repository<CourseTask>, ICourseTaskRepository
{
    public CourseTaskRepository(LMSDbContext context)
        : base(context)
    {
    }
}