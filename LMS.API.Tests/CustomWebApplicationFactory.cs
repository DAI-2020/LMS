using LMS.API.Data;
using LMS.API.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LMS.API.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private static readonly string DbName = $"TestDb_{Guid.NewGuid()}";

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<LMSDbContext>));
            if (descriptor != null) services.Remove(descriptor);

            services.AddDbContext<LMSDbContext>(options =>
                options.UseInMemoryDatabase(DbName));
        });

        var host = base.CreateHost(builder);

        using var scope = host.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<LMSDbContext>();
        db.Database.EnsureCreated();
        SeedTestData(db);

        return host;
    }

    private static void SeedTestData(LMSDbContext db)
    {
        if (db.Users.Any()) return;

        var studentRole = new Role { Id = 1, Name = "Student" };
        var instructorRole = new Role { Id = 2, Name = "Instructor" };
        var adminRole = new Role { Id = 3, Name = "Admin" };
        db.Roles.AddRange(studentRole, instructorRole, adminRole);

        var user = new User
        {
            Id = 1,
            FullName = "Ahmed Student",
            Email = "ahmedstudent@gmail.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
            PhoneNumber = "01234567890",
            CreatedAt = DateTime.UtcNow
        };
        db.Users.Add(user);
        db.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = studentRole.Id });

        var course = new Course { Id = 1, Title = "C# Programming", Description = "Learn C#" };
        db.Courses.Add(course);

        var task = new CourseTask
        {
            Id = 1,
            Title = "Assignment 1",
            Description = "First assignment",
            CourseId = 1,
            DueDate = DateTime.UtcNow.AddDays(7),
            TaskType = LMS.API.Enums.TasksEnums.TaskType.TechTask
        };
        db.CourseTasks.Add(task);

        var session = new LiveSession
        {
            Id = 1,
            CourseId = 1,
            Title = "Session 1",
            ScheduledAt = DateTime.UtcNow.AddDays(-1),
            DurationMinutes = 60,
            Status = LMS.API.Enums.LiveSession.LiveSessionStatus.Completed,
            Type = LMS.API.Enums.LiveSession.LiveSessionType.Lecture,
            Mode = LMS.API.Enums.LiveSession.DeliveryMode.Online,
            WeekNumber = 1
        };
        db.LiveSessions.Add(session);

        db.FAQs.Add(new FAQ
        {
            Question = "How do I reset my password?",
            Answer = "Go to settings and click change password."
        });

        db.SaveChanges();
    }
}
