using LMS.API.Models;

namespace LMS.API.Repositories.Interfaces;

public interface IQuizRepository : IRepository<Quiz>
{
    Task<IEnumerable<Quiz>> GetRecentQuizzesByStudentAsync(int studentId);

    Task<IEnumerable<Quiz>> GetByTopicAsync(int topicId);
}