using LMS.API.Data;
using LMS.API.Models;
using LMS.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS.API.Repositories.Implementations;

public class GraduationProjectRepository: Repository<GraduationProjectSubmission>,IGraduationProjectRepository
{
    private readonly LMSDbContext _context;

    public GraduationProjectRepository(LMSDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<GraduationProjectSubmission?> GetByStudentIdAsync(int studentId)
    {
        return await _context.GraduationProjectSubmissions
            .Include(x => x.Student)
            .FirstOrDefaultAsync(x => x.StudentId == studentId);
    }
}