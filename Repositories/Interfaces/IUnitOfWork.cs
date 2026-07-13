using LMS.API.Models;
namespace LMS.API.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }

        ICourseRepository Courses { get; }

        IMaterialRepository Materials { get; }

        ICourseTaskRepository CourseTasks { get; }

        ITaskSubmissionRepository TaskSubmissions { get; }

        IAIChatRepository AIChats { get; }

        IGraduationProjectRepository GraduationProjects { get; }

        IQuizRepository Quizzes { get; }

        ITopicRepository Topics { get; }

        Task<int> SaveChangesAsync();
    }
}
