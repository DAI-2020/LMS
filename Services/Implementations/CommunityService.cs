using LMS.API.DTOs.Community;
using LMS.API.Models;
using LMS.API.Repositories.Interfaces;
using LMS.API.Services.Interfaces;

namespace LMS.API.Services.Implementations
{
    public class CommunityService : ICommunityService
    {
        private readonly ICommunityPostRepository _repository;

        public CommunityService(ICommunityPostRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CommunityPostDto>> GetAllAsync()
        {
            var posts = await _repository.GetAllAsync();
            return posts.Select(MapToDto);
        }

        public async Task<CommunityPostDto?> GetByIdAsync(int id)
        {
            var post = await _repository.GetByIdAsync(id);
            return post is null ? null : MapToDto(post);
        }

        public async Task<IEnumerable<CommunityPostDto>> GetRecentPostsAsync(int count)
        {
            var posts = await _repository.GetRecentPostsAsync(count);
            return posts.Select(MapToDto);
        }

        public async Task<CommunityPostDto> CreateAsync(int userId, string content)
        {
            var post = new CommunityPost
            {
                UserId = userId,
                Content = content,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _repository.AddAsync(post);
            return MapToDto(created);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var post = await _repository.GetByIdAsync(id);
            if (post is null) return false;
            await _repository.DeleteAsync(id);
            return true;
        }

        private static CommunityPostDto MapToDto(CommunityPost post)
        {
            return new CommunityPostDto
            {
                Id = post.Id,
                UserId = post.UserId,
                UserName = post.User?.FullName ?? string.Empty,
                Content = post.Content,
                CreatedAt = post.CreatedAt
            };
        }
    }
}
