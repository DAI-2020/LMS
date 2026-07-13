
using LMS.API.Data;
using LMS.API.Repositories.Implementations;
using LMS.API.Repositories.Interfaces;
using LMS.API.Services.Interfaces;
using LMS.API.Services.Implementations;
using Microsoft.EntityFrameworkCore;

namespace LMS.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add DbContext
            builder.Services.AddDbContext<LMSDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add HttpClient for AI Service
            builder.Services.AddHttpClient<IAIService, AIService>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Repositories - Person A
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ICourseRepository, CourseRepository>();
            builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
            builder.Services.AddScoped<ICourseTaskRepository, CourseTaskRepository>();
            builder.Services.AddScoped<ITaskSubmissionRepository, TaskSubmissionRepository>();
            builder.Services.AddScoped<IAIChatRepository, AIChatRepository>();
            builder.Services.AddScoped<IGraduationProjectRepository, GraduationProjectRepository>();
            builder.Services.AddScoped<ITopicRepository, TopicRepository>();
            builder.Services.AddScoped<IQuizRepository, QuizRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Repositories - Person B (to be added by Person B)
            // builder.Services.AddScoped<IAttendanceLogRepository, AttendanceLogRepository>();
            // builder.Services.AddScoped<ILiveSessionRepository, LiveSessionRepository>();
            // builder.Services.AddScoped<ICommunityPostRepository, CommunityPostRepository>();
            // builder.Services.AddScoped<ITicketRepository, TicketRepository>();
            // builder.Services.AddScoped<ITicketReplyRepository, TicketReplyRepository>();

            // Services - Person A
            builder.Services.AddScoped<IMaterialService, MaterialService>();
            builder.Services.AddScoped<ICourseTaskService, CourseTaskService>();
            builder.Services.AddScoped<ICourseService, CourseService>();
            builder.Services.AddScoped<ITaskAndPerformanceService, TaskAndPerformanceService>();
            builder.Services.AddScoped<IGraduationProjectService, GraduationProjectService>();
            builder.Services.AddScoped<IAiAssistantService, AiAssistantService>();
            builder.Services.AddScoped<IFileService, FileService>();

            // Services - Person B (to be added by Person B)
            // builder.Services.AddScoped<IAttendanceService, AttendanceService>();
            // builder.Services.AddScoped<ILiveSessionService, LiveSessionService>();
            // builder.Services.AddScoped<ICommunityService, CommunityService>();
            // builder.Services.AddScoped<ITicketService, TicketService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
