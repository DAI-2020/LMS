using LMS.API.Data;
using LMS.API.Models;
using LMS.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS.API.Repositories.Implementations;

public class QuizRepository: Repository<Quiz>,IQuizRepository
{
    private readonly LMSDbContext _context;

    public QuizRepository(LMSDbContext context)
        : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Quiz>> GetRecentQuizzesByStudentAsync(int studentId)
    {
        return await _context.Quizzes
            .Where(x => x.StudentId == studentId)
            .OrderByDescending(x => x.TakenAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Quiz>> GetByTopicAsync(int topicId)
    {
        return await _context.Quizzes
            .Where(x => x.TopicId == topicId)
            .ToListAsync();
    }
}