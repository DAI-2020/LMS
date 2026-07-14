using LMS.API.Models;

namespace LMS.API.Repositories.Interfaces;

public interface IQuizRepository : IRepository<Quiz>
{
    Task<IEnumerable<Quiz>> GetByStudentIdAsync(int studentId);

    Task<IEnumerable<Quiz>> GetByCourseIdAsync(int courseId);

    Task<IEnumerable<Quiz>> GetByStudentAndCourseAsync(int studentId, int courseId);

    Task<IEnumerable<Quiz>> GetAllWithTopicAsync();
}
