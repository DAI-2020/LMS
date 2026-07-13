using LMS.API.Data;
using LMS.API.Models;
using LMS.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS.API.Repositories.Implementations;

public class GraduationProjectRepository : Repository<GraduationProjectSubmission>, IGraduationProjectRepository
{
    public GraduationProjectRepository(LMSDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<GraduationProjectSubmission>> GetByStudentIdAsync(int studentId)
    {
        return await _dbSet
            .Where(x => x.StudentId == studentId)
            .OrderByDescending(x => x.SubmittedAt)
            .ToListAsync();
    }
}
