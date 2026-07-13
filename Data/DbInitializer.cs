using LMS.API.Data;
using LMS.API.Enums.User;
using LMS.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LMS.API.Data;

public static class DbInitializer
{
    public static void Initialize(LMSDbContext context)
    {
        context.Database.EnsureCreated();

        if (context.Users.Any())
            return;

        SeedRoles(context);
        SeedUsers(context);
        SeedCourses(context);
        SeedTopics(context);
        SeedFaqs(context);

        context.SaveChanges();
    }

    private static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private static void SeedRoles(LMSDbContext context)
    {
        var roles = new[]
        {
            new Role { Id = (int)RoleType.Admin, Name = RoleType.Admin.ToString() },
            new Role { Id = (int)RoleType.Instructor, Name = RoleType.Instructor.ToString() },
            new Role { Id = (int)RoleType.Student, Name = RoleType.Student.ToString() }
        };
        context.Roles.AddRange(roles);
    }

    private static void SeedUsers(LMSDbContext context)
    {
        var password = "pasword123";
        var hashedPassword = HashPassword(password);

        var admin1 = new User
        {
            FullName = "Mohamed Samy",
            Email = "mohamedsamy@gmail.com",
            PasswordHash = hashedPassword,
            CreatedAt = DateTime.UtcNow,
            Gender = Gender.Male
        };

        var admin2 = new User
        {
            FullName = "Dai Ahmed",
            Email = "daiahmed@gmail.com",
            PasswordHash = hashedPassword,
            CreatedAt = DateTime.UtcNow,
            Gender = Gender.Male
        };

        var instructor = new User
        {
            FullName = "Dai Instructor",
            Email = "daiinstructor@gmail.com",
            PasswordHash = hashedPassword,
            CreatedAt = DateTime.UtcNow,
            Gender = Gender.Male
        };

        var instructorTest = new User
        {
            FullName = "Instructor Test",
            Email = "instructor_test@example.com",
            PasswordHash = hashedPassword,
            CreatedAt = DateTime.UtcNow,
            Gender = Gender.Male
        };

        var student = new User
        {
            FullName = "Ahmed Student",
            Email = "ahmedstudent@gmail.com",
            PasswordHash = hashedPassword,
            CreatedAt = DateTime.UtcNow,
            Gender = Gender.Male
        };

        context.Users.AddRange(admin1, admin2, instructor, instructorTest, student);
        context.SaveChanges();

        context.UserRoles.AddRange(
            new UserRole { UserId = admin1.Id, RoleId = (int)RoleType.Admin },
            new UserRole { UserId = admin2.Id, RoleId = (int)RoleType.Admin },
            new UserRole { UserId = instructor.Id, RoleId = (int)RoleType.Instructor },
            new UserRole { UserId = instructorTest.Id, RoleId = (int)RoleType.Instructor },
            new UserRole { UserId = student.Id, RoleId = (int)RoleType.Student }
        );
    }

    private static void SeedCourses(LMSDbContext context)
    {
        var instructor = context.Users.First(u => u.Email == "daiinstructor@gmail.com");

        var courses = new[]
        {
            new Course
            {
                Title = "Introduction to Computer Science",
                Description = "Fundamentals of computer science and programming",
                InstructorId = instructor.Id
            },
            new Course
            {
                Title = "Data Structures & Algorithms",
                Description = "Advanced data structures and algorithm design",
                InstructorId = instructor.Id
            },
            new Course
            {
                Title = "Web Development",
                Description = "Full-stack web development with ASP.NET Core",
                InstructorId = instructor.Id
            }
        };
        context.Courses.AddRange(courses);
    }

    private static void SeedTopics(LMSDbContext context)
    {
        var topics = new[]
        {
            new Topic { Name = "Variables & Data Types" },
            new Topic { Name = "Control Flow" },
            new Topic { Name = "Object-Oriented Programming" },
            new Topic { Name = "Arrays & Lists" },
            new Topic { Name = "Sorting Algorithms" },
            new Topic { Name = "HTML & CSS Basics" },
            new Topic { Name = "ASP.NET Core MVC" }
        };
        context.Topics.AddRange(topics);
    }

    private static void SeedFaqs(LMSDbContext context)
    {
        var faqs = new[]
        {
            new FAQ
            {
                Question = "How do I reset my password?",
                Answer = "Go to Profile > Change Password and enter your old and new password."
            },
            new FAQ
            {
                Question = "How do I join a live session?",
                Answer = "Navigate to Live Sessions, find your session, and click Join."
            },
            new FAQ
            {
                Question = "Can I submit homework after the deadline?",
                Answer = "No, homework submissions after the deadline are not accepted."
            }
        };
        context.FAQs.AddRange(faqs);
    }
}
