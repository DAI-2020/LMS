using LMS.API.Data;
using LMS.API.Repositories.Interfaces;

namespace LMS.API.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LMSDbContext _context;

        public IUserRepository Users { get; }
        public ICourseRepository Courses { get; }
        public IMaterialRepository Materials { get; }
        public ICourseTaskRepository CourseTasks { get; }
        public ITaskSubmissionRepository TaskSubmissions { get; }
        public IAIChatRepository AIChats { get; }

        public UnitOfWork(
            LMSDbContext context,
            IUserRepository users,
            ICourseRepository courses,
            IMaterialRepository materials,
            ICourseTaskRepository courseTasks,
            ITaskSubmissionRepository taskSubmissions,
            IAIChatRepository aiChats)
        {
            _context = context;
            Users = users;
            Courses = courses;
            Materials = materials;
            CourseTasks = courseTasks;
            TaskSubmissions = taskSubmissions;
            AIChats = aiChats;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
