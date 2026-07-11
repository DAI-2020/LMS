using LMS.API.DTOs.Community;

namespace LMS.API.Services.Interfaces
{
    public interface ICommunityService
    {
        Task<IEnumerable<CommunityPostDto>> GetAllAsync();
        Task<CommunityPostDto?> GetByIdAsync(int id);
        Task<IEnumerable<CommunityPostDto>> GetRecentPostsAsync(int count);
        Task<CommunityPostDto> CreateAsync(int userId, string content);
        Task<bool> DeleteAsync(int id);
    }
}
