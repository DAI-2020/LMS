using LMS.API.Data;
using LMS.API.Repositories.Implementations;
using LMS.API.Repositories.Interfaces;
using LMS.API.Services.Implementations;
using LMS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<LMSDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ICourseRepository, CourseRepository>();
            builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
            builder.Services.AddScoped<ICourseTaskRepository, CourseTaskRepository>();
            builder.Services.AddScoped<ITaskSubmissionRepository, TaskSubmissionRepository>();
            builder.Services.AddScoped<IAIChatRepository, AIChatRepository>();
            builder.Services.AddScoped<ILiveSessionRepository, LiveSessionRepository>();
            builder.Services.AddScoped<IAttendanceLogRepository, AttendanceLogRepository>();
            builder.Services.AddScoped<IAttendanceRepository, AttendanceRepository>();
            builder.Services.AddScoped<ITicketRepository, TicketRepository>();
            builder.Services.AddScoped<ITicketReplyRepository, TicketReplyRepository>();
            builder.Services.AddScoped<ICommunityPostRepository, CommunityPostRepository>();
            builder.Services.AddScoped<IFaqRepository, FaqRepository>();
            builder.Services.AddScoped<IUserDeviceRepository, UserDeviceRepository>();
            builder.Services.AddScoped<INotificationPreferenceRepository, NotificationPreferenceRepository>();

            builder.Services.AddScoped<IGraduationProjectRepository, GraduationProjectRepository>();
            builder.Services.AddScoped<IQuizRepository, QuizRepository>();
            builder.Services.AddScoped<ITopicRepository, TopicRepository>();

            builder.Services.AddScoped<ILiveSessionService, LiveSessionService>();
            builder.Services.AddScoped<IAttendanceService, AttendanceService>();
            builder.Services.AddScoped<IAttendanceSummaryService, AttendanceSummaryService>();
            builder.Services.AddScoped<ITicketService, TicketService>();
            builder.Services.AddScoped<ICommunityService, CommunityService>();
            builder.Services.AddScoped<IFaqService, FaqService>();
            builder.Services.AddScoped<IUserDeviceService, UserDeviceService>();
            builder.Services.AddScoped<IProfileAndSecurityService, ProfileAndSecurityService>();

            builder.Services.AddScoped<IFileService, FileService>();
            builder.Services.AddScoped<IAIService, AIService>();
            builder.Services.AddScoped<IGraduationProjectService, GraduationProjectService>();
            builder.Services.AddScoped<ITaskAndPerformanceService, TaskAndPerformanceService>();
            builder.Services.AddScoped<IAiAssistantService, AiAssistantService>();

            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
