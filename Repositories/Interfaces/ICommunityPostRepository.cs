using LMS.API.Models;

namespace LMS.API.Repositories.Interfaces
{
    public interface ICommunityPostRepository
    {
        Task<IEnumerable<CommunityPost>> GetAllAsync();
        Task<CommunityPost?> GetByIdAsync(int id);
        Task<IEnumerable<CommunityPost>> GetRecentPostsAsync(int count);
        Task<CommunityPost> AddAsync(CommunityPost post);
        Task DeleteAsync(int id);
    }
}
