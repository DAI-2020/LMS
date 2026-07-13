using LMS.API.DTOs.Task;
using LMS.API.Enums.TasksEnums;
using LMS.API.Models;
using LMS.API.Repositories.Interfaces;
using LMS.API.Services.Interfaces;

namespace LMS.API.Services.Implementations
{
    public class InstructorTaskService : IInstructorTaskService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InstructorTaskService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TaskResponseDto?> GetByIdAsync(int id)
        {
            var task = await _unitOfWork.CourseTasks.GetByIdAsync(id);
            return task is null ? null : MapToResponse(task);
        }

        public async Task<IEnumerable<TaskResponseDto>> GetByCourseIdAsync(int courseId)
        {
            var allTasks = await _unitOfWork.CourseTasks.GetAllAsync();
            return allTasks
                .Where(t => t.CourseId == courseId)
                .Select(MapToResponse);
        }

        public async Task<TaskResponseDto> CreateAsync(CreateTaskDto dto)
        {
            if (!Enum.TryParse<TaskType>(dto.TaskType, true, out var taskType))
                throw new ArgumentException($"Invalid TaskType: {dto.TaskType}");

            var courseExists = await _unitOfWork.Courses.GetByIdAsync(dto.CourseId);
            if (courseExists is null)
                throw new ArgumentException($"Course with Id {dto.CourseId} not found.");

            var task = new CourseTask
            {
                CourseId = dto.CourseId,
                SessionId = dto.SessionId,
                Title = dto.Title,
                Description = dto.Description,
                TaskType = taskType,
                AssignmentStatus = dto.AssignmentStatus,
                DueDate = dto.DueDate,
                AllowedExtensions = dto.AllowedExtensions
            };

            await _unitOfWork.CourseTasks.AddAsync(task);
            await _unitOfWork.SaveChangesAsync();

            return MapToResponse(task);
        }

        public async Task<TaskResponseDto?> UpdateAsync(int id, UpdateTaskDto dto)
        {
            var task = await _unitOfWork.CourseTasks.GetByIdAsync(id);
            if (task is null) return null;

            if (dto.Title is not null) task.Title = dto.Title;
            if (dto.Description is not null) task.Description = dto.Description;
            if (dto.DueDate != default) task.DueDate = dto.DueDate;
            if (dto.AllowedExtensions is not null) task.AllowedExtensions = dto.AllowedExtensions;

            _unitOfWork.CourseTasks.Update(task);
            await _unitOfWork.SaveChangesAsync();

            return MapToResponse(task);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var task = await _unitOfWork.CourseTasks.GetByIdAsync(id);
            if (task is null) return false;

            _unitOfWork.CourseTasks.Delete(task);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        private static TaskResponseDto MapToResponse(CourseTask t)
        {
            return new TaskResponseDto
            {
                Id = t.Id,
                CourseId = t.CourseId,
                SessionId = t.SessionId,
                Title = t.Title,
                Description = t.Description,
                TaskType = t.TaskType.ToString(),
                AssignmentStatus = t.AssignmentStatus,
                DueDate = t.DueDate,
                AllowedExtensions = t.AllowedExtensions
            };
        }
    }
}
