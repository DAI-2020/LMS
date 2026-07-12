using LMS.API.DTOs.CourseTask;
using LMS.API.Models;
using LMS.API.Repositories.Interfaces;
using LMS.API.Services.Interfaces;

namespace LMS.API.Services.Implementations;

public class CourseTaskService : ICourseTaskService
{
    private readonly IUnitOfWork _unitOfWork;

    public CourseTaskService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<IEnumerable<CourseTaskResponseDto>> GetAllAsync()
    {
        var tasks = await _unitOfWork.CourseTasks.GetAllAsync();

        return tasks.Select(t => new CourseTaskResponseDto
        {
            Id = t.Id,
            CourseId = t.CourseId,
            SessionId = t.SessionId,
            Title = t.Title,
            Description = t.Description,
            TaskType = t.TaskType,
            AssignmentStatus = t.AssignmentStatus,
            DueDate = t.DueDate,
            AllowedExtensions = t.AllowedExtensions
        });
    }
    public async Task<CourseTaskResponseDto?> GetByIdAsync(int id)
    {
        var task = await _unitOfWork.CourseTasks.GetByIdAsync(id);

        if (task == null)
            return null;

        return new CourseTaskResponseDto
        {
            Id = task.Id,
            CourseId = task.CourseId,
            SessionId = task.SessionId,
            Title = task.Title,
            Description = task.Description,
            TaskType = task.TaskType,
            AssignmentStatus = task.AssignmentStatus,
            DueDate = task.DueDate,
            AllowedExtensions = task.AllowedExtensions
        };
    }
    public async Task AddAsync(CreateCourseTaskDto dto)
    {
        var task = new CourseTask
        {
            CourseId = dto.CourseId,
            SessionId = dto.SessionId,
            Title = dto.Title,
            Description = dto.Description,
            TaskType = dto.TaskType,
            AssignmentStatus = dto.AssignmentStatus,
            DueDate = dto.DueDate,
            AllowedExtensions = dto.AllowedExtensions
        };

        await _unitOfWork.CourseTasks.AddAsync(task);

        await _unitOfWork.SaveChangesAsync();
    }
    public async Task UpdateAsync(int id, UpdateCourseTaskDto dto)
    {
        var task = await _unitOfWork.CourseTasks.GetByIdAsync(id);

        if (task == null)
            throw new Exception("Task not found.");

        task.Title = dto.Title;
        task.Description = dto.Description;
        task.TaskType = dto.TaskType;
        task.AssignmentStatus = dto.AssignmentStatus;
        task.DueDate = dto.DueDate;
        task.AllowedExtensions = dto.AllowedExtensions;

        _unitOfWork.CourseTasks.Update(task);

        await _unitOfWork.SaveChangesAsync();
    }
    public async Task DeleteAsync(int id)
    {
        var task = await _unitOfWork.CourseTasks.GetByIdAsync(id);

        if (task == null)
            throw new Exception("Task not found.");

        _unitOfWork.CourseTasks.Delete(task);

        await _unitOfWork.SaveChangesAsync();
    }
}
