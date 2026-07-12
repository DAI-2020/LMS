using LMS.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LMS.API.Data;

public class LMSDbContext : DbContext
{
    public LMSDbContext(DbContextOptions<LMSDbContext> options)
        : base(options)
    {
    }
    // Add By Dai Ahmed
    // Users & Roles
    public DbSet<User> Users { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<UserRole> UserRoles { get; set; }
    // Courses
    public DbSet<Course> Courses { get; set; }

    public DbSet<Material> Materials { get; set; }

    public DbSet<Models.CourseTask> CourseTasks { get; set; }

    public DbSet<TaskSubmission> TaskSubmissions { get; set; }
    
    public DbSet<GraduationProjectSubmission> GraduationProjectSubmissions { get; set; } 
   
    public DbSet<Topic> Topics { get; set; }
   
    public DbSet<Quiz> Quizzes { get; set; }
    // AI Assistant
    public DbSet<AIAssistantChat> AIAssistantChats { get; set; }

    //Add By Mohamed Samy
    public DbSet<LiveSession> LiveSessions { get; set; }
    public DbSet<AttendanceLog> AttendanceLogs { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<TicketReply> TicketReplies { get; set; }
    public DbSet<CommunityPost> CommunityPosts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LMSDbContext).Assembly);
    }
}
