using LMS.API.Data;
using LMS.API.Repositories.Interfaces;

namespace LMS.API.Repositories.Implementations;

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
    public ILiveSessionRepository LiveSessions { get; }
    public IAttendanceLogRepository AttendanceLogs { get; }
    public IAttendanceRepository Attendance { get; }
    public ITicketRepository Tickets { get; }
    public ITicketReplyRepository TicketReplies { get; }
    public ICommunityPostRepository CommunityPosts { get; }
    public IFaqRepository Faqs { get; }
    public IUserDeviceRepository UserDevices { get; }
    public INotificationPreferenceRepository NotificationPreferences { get; }

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
        ITopicRepository topics,
        ILiveSessionRepository liveSessions,
        IAttendanceLogRepository attendanceLogs,
        IAttendanceRepository attendance,
        ITicketRepository tickets,
        ITicketReplyRepository ticketReplies,
        ICommunityPostRepository communityPosts,
        IFaqRepository faqs,
        IUserDeviceRepository userDevices,
        INotificationPreferenceRepository notificationPreferences)
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
        LiveSessions = liveSessions;
        AttendanceLogs = attendanceLogs;
        Attendance = attendance;
        Tickets = tickets;
        TicketReplies = ticketReplies;
        CommunityPosts = communityPosts;
        Faqs = faqs;
        UserDevices = userDevices;
        NotificationPreferences = notificationPreferences;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
