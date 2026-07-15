namespace LMS.API.Repositories.Interfaces;

public interface IUnitOfWork : IDisposable
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
    ILiveSessionRepository LiveSessions { get; }
    IAttendanceLogRepository AttendanceLogs { get; }
    IAttendanceRepository Attendance { get; }
    ITicketRepository Tickets { get; }
    ITicketReplyRepository TicketReplies { get; }
    IFaqRepository Faqs { get; }
    IUserDeviceRepository UserDevices { get; }
    INotificationPreferenceRepository NotificationPreferences { get; }
    Task<int> SaveChangesAsync();
}
