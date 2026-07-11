using Microsoft.EntityFrameworkCore;
using LMS.API.Data;
using LMS.API.Models;
using LMS.API.Repositories.Interfaces;

namespace LMS.API.Repositories.Implementations
{
    public class CommunityPostRepository : ICommunityPostRepository
    {
        private readonly LMSDbContext _context;
        public CommunityPostRepository(LMSDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CommunityPost>> GetAllAsync()
        {
            return await _context.CommunityPosts
                .Include(p => p.User)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<CommunityPost?> GetByIdAsync(int id)
        {
            return await _context.CommunityPosts
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<CommunityPost>> GetRecentPostsAsync(int count)
        {
            return await _context.CommunityPosts
                .Include(p => p.User)
                .OrderByDescending(p => p.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<CommunityPost> AddAsync(CommunityPost post)
        {
            await _context.CommunityPosts.AddAsync(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task DeleteAsync(int id)
        {
            var post = await _context.CommunityPosts.FindAsync(id);
            if (post is not null)
            {
                _context.CommunityPosts.Remove(post);
                await _context.SaveChangesAsync();
            }
        }
    }
}
