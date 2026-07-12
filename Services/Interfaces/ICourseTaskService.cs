using LMS.API.DTOs.CourseTask;

namespace LMS.API.Services.Interfaces;

public interface ICourseTaskService
{
    Task<IEnumerable<CourseTaskResponseDto>> GetAllAsync();

    Task<CourseTaskResponseDto?> GetByIdAsync(int id);

    Task AddAsync(CreateCourseTaskDto dto);

    Task UpdateAsync(int id, UpdateCourseTaskDto dto);

    Task DeleteAsync(int id);
}