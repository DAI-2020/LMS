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

        public IGraduationProjectRepository GraduationProjects { get; }

        public IQuizRepository Quizzes { get; }

        public ITopicRepository Topics { get; }

        public UnitOfWork(
            LMSDbContext context,
            IUserRepository users,
            ICourseRepository courses,
            IMaterialRepository materials,
            ICourseTaskRepository courseTasks,
            ITaskSubmissionRepository taskSubmissions,
            IAIChatRepository aiChats,
            IGraduationProjectRepository graduationProjects,
            IQuizRepository quizzes,
            ITopicRepository topics)
        {
            _context = context;
            Users = users;
            Courses = courses;
            Materials = materials;
            CourseTasks = courseTasks;
            TaskSubmissions = taskSubmissions;
            AIChats = aiChats;
            GraduationProjects = graduationProjects;
            Quizzes = quizzes;
            Topics = topics;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
