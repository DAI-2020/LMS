using LMS.API.DTOs.Task;

namespace LMS.API.Services.Interfaces
{
    public interface IInstructorTaskService
    {
        Task<TaskResponseDto?> GetByIdAsync(int id);
        Task<IEnumerable<TaskResponseDto>> GetByCourseIdAsync(int courseId);
        Task<TaskResponseDto> CreateAsync(CreateTaskDto dto);
        Task<TaskResponseDto?> UpdateAsync(int id, UpdateTaskDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
