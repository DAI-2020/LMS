using LMS.API.Models;

namespace LMS.API.Repositories.Interfaces;

public interface ITopicRepository : IRepository<Topic>
{
    Task<IEnumerable<Topic>> GetTopicsByCourseAsync(int courseId);
}