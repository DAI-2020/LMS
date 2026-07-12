
using LMS.API.Repositories.Implementations;
using LMS.API.Repositories.Interfaces;
using LMS.API.Services.Interfaces;
using LMS.API.Services.Implementations;

namespace LMS.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
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

            builder.Services.AddScoped<IMaterialService, MaterialService>();
            
            builder.Services.AddScoped<ICourseTaskService, CourseTaskService>();
            
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
