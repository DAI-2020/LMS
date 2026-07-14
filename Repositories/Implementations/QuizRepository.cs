using LMS.API.Data;
using LMS.API.Models;
using LMS.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS.API.Repositories.Implementations;

public class QuizRepository : Repository<Quiz>, IQuizRepository
{
    public QuizRepository(LMSDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<Quiz>> GetByStudentIdAsync(int studentId)
    {
        return await _dbSet
            .Where(x => x.StudentId == studentId)
            .Include(x => x.Topic)
            .Include(x => x.Course)
            .ToListAsync();
    }

    public async Task<IEnumerable<Quiz>> GetByCourseIdAsync(int courseId)
    {
        return await _dbSet
            .Where(x => x.CourseId == courseId)
            .Include(x => x.Topic)
            .Include(x => x.Student)
            .ToListAsync();
    }

    public async Task<IEnumerable<Quiz>> GetByStudentAndCourseAsync(int studentId, int courseId)
    {
        return await _dbSet
            .Where(x => x.StudentId == studentId && x.CourseId == courseId)
            .Include(x => x.Topic)
            .ToListAsync();
    }

    public async Task<IEnumerable<Quiz>> GetAllWithTopicAsync()
    {
        return await _dbSet
            .Include(x => x.Topic)
            .ToListAsync();
    }
}
