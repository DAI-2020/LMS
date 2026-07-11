using LMS.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LMS.API.Data;

public class LMSDbContext : DbContext
{
    public LMSDbContext(DbContextOptions<LMSDbContext> options)
        : base(options)
    {
    }
    // Users & Roles
    public DbSet<User> Users { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<UserRole> UserRoles { get; set; }
    // Courses
    public DbSet<Course> Courses { get; set; }

    public DbSet<Material> Materials { get; set; }

    public DbSet<Models.CourseTask> CourseTasks { get; set; }

    public DbSet<TaskSubmission> TaskSubmissions { get; set; }
    // AI Assistant
    public DbSet<AIAssistantChat> AIAssistantChats { get; set; }

  
        protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(LMSDbContext).Assembly);
    }
}
