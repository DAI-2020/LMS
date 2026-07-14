using LMS.API.Data;
using LMS.API.Enums.User;
using LMS.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LMS.API.Data;

public static class DbInitializer
{
    public static void Initialize(LMSDbContext context)
    {
        Console.WriteLine("--> Database Seeding process started...");

        context.Database.EnsureCreated();

        SeedRoles(context);
        SeedUsers(context);
        SeedUserRoles(context);
        SeedCourses(context);
        SeedTopics(context);
        SeedFaqs(context);

        context.SaveChanges();

        Console.WriteLine("--> Database Seeding completed successfully!");
    }

    private static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private static void SeedRoles(LMSDbContext context)
    {
        if (context.Roles.Any())
        {
            Console.WriteLine($"--> Roles already exist ({context.Roles.Count()}). Skipping.");
            return;
        }

        Console.WriteLine("--> Seeding Roles...");
        context.Roles.AddRange(
            new Role { Id = (int)RoleType.Admin, Name = RoleType.Admin.ToString() },
            new Role { Id = (int)RoleType.Instructor, Name = RoleType.Instructor.ToString() },
            new Role { Id = (int)RoleType.Student, Name = RoleType.Student.ToString() }
        );
        context.SaveChanges();
        Console.WriteLine($"--> Seeding Roles... Done. Added {context.Roles.Count()} roles.");
    }

    private static void SeedUsers(LMSDbContext context)
    {
        if (context.Users.Any())
        {
            Console.WriteLine($"--> Users already exist ({context.Users.Count()}). Skipping.");
            return;
        }

        Console.WriteLine("--> Seeding Users...");
        var password = "password123";
        var hashedPassword = HashPassword(password);

        context.Users.AddRange(
            new User
            {
                FullName = "Mohamed Samy",
                Email = "mohamedsamy@lms.com",
                PasswordHash = hashedPassword,
                CreatedAt = DateTime.UtcNow,
                Gender = Gender.Male
            },
            new User
            {
                FullName = "Dai Ahmed",
                Email = "daiahmed@lms.com",
                PasswordHash = hashedPassword,
                CreatedAt = DateTime.UtcNow,
                Gender = Gender.Male
            },
            new User
            {
                FullName = "Dai Instructor",
                Email = "daiinstructor@lms.com",
                PasswordHash = hashedPassword,
                CreatedAt = DateTime.UtcNow,
                Gender = Gender.Male
            },
            new User
            {
                FullName = "Instructor Test",
                Email = "instructortest@lms.com",
                PasswordHash = hashedPassword,
                CreatedAt = DateTime.UtcNow,
                Gender = Gender.Male
            },
            new User
            {
                FullName = "Ahmed Student",
                Email = "ahmedstudent@lms.com",
                PasswordHash = hashedPassword,
                CreatedAt = DateTime.UtcNow,
                Gender = Gender.Male
            }
        );
        context.SaveChanges();
        Console.WriteLine($"--> Seeding Users... Done. Added {context.Users.Count()} users.");
    }

    private static void SeedUserRoles(LMSDbContext context)
    {
        if (context.UserRoles.Any())
        {
            Console.WriteLine($"--> UserRoles already exist ({context.UserRoles.Count()}). Skipping.");
            return;
        }

        Console.WriteLine("--> Seeding UserRoles...");
        var users = context.Users.ToDictionary(u => u.Email);
        var roles = context.Roles.ToDictionary(r => r.Name);

        context.UserRoles.AddRange(
            new UserRole { UserId = users["mohamedsamy@lms.com"].Id, RoleId = roles["Admin"].Id },
            new UserRole { UserId = users["daiahmed@lms.com"].Id, RoleId = roles["Admin"].Id },
            new UserRole { UserId = users["daiinstructor@lms.com"].Id, RoleId = roles["Instructor"].Id },
            new UserRole { UserId = users["instructortest@lms.com"].Id, RoleId = roles["Instructor"].Id },
            new UserRole { UserId = users["ahmedstudent@lms.com"].Id, RoleId = roles["Student"].Id }
        );
        context.SaveChanges();
        Console.WriteLine($"--> Seeding UserRoles... Done. Added {context.UserRoles.Count()} mappings.");
    }

    private static void SeedCourses(LMSDbContext context)
    {
        if (context.Courses.Any())
        {
            Console.WriteLine($"--> Courses already exist ({context.Courses.Count()}). Skipping.");
            return;
        }

        Console.WriteLine("--> Seeding Courses...");
        var instructor = context.Users.First(u => u.Email == "daiinstructor@lms.com");

        context.Courses.AddRange(
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
        );
        context.SaveChanges();
        Console.WriteLine($"--> Seeding Courses... Done. Added {context.Courses.Count()} courses.");
    }

    private static void SeedTopics(LMSDbContext context)
    {
        if (context.Topics.Any())
        {
            Console.WriteLine($"--> Topics already exist ({context.Topics.Count()}). Skipping.");
            return;
        }

        Console.WriteLine("--> Seeding Topics...");
        context.Topics.AddRange(
            new Topic { Name = "Variables & Data Types" },
            new Topic { Name = "Control Flow" },
            new Topic { Name = "Object-Oriented Programming" },
            new Topic { Name = "Arrays & Lists" },
            new Topic { Name = "Sorting Algorithms" },
            new Topic { Name = "HTML & CSS Basics" },
            new Topic { Name = "ASP.NET Core MVC" }
        );
        context.SaveChanges();
        Console.WriteLine($"--> Seeding Topics... Done. Added {context.Topics.Count()} topics.");
    }

    private static void SeedFaqs(LMSDbContext context)
    {
        if (context.FAQs.Any())
        {
            Console.WriteLine($"--> FAQs already exist ({context.FAQs.Count()}). Skipping.");
            return;
        }

        Console.WriteLine("--> Seeding FAQs...");
        context.FAQs.AddRange(
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
        );
        context.SaveChanges();
        Console.WriteLine($"--> Seeding FAQs... Done. Added {context.FAQs.Count()} FAQs.");
    }
}
