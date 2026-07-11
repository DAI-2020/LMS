using LMS.API.Data;
using LMS.API.Models;
using LMS.API.Repositories.Interfaces;

namespace LMS.API.Repositories.Implementations;

public class CourseRepository : Repository<Course>, ICourseRepository
{
    public CourseRepository(LMSDbContext context)
        : base(context)
    {
    }
}