using LMS.API.Models;

namespace LMS.API.Repositories.Interfaces;

public interface ITopicRepository : IRepository<Topic>
{
    Task<Topic?> GetByNameAsync(string name);
}
