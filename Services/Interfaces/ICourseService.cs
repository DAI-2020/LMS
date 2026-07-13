using LMS.API.DTOs.Course;
using LMS.API.DTOs.Material;

namespace LMS.API.Services.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseResponseDto>> GetAllAsync();

        Task<CourseResponseDto?> GetByIdAsync(int id);

        Task AddAsync(CreateCourseDto dto);

        Task UpdateAsync(int id, UpdateCourseDto dto);

        Task DeleteAsync(int id);

        Task<IEnumerable<MaterialResponseDto>> GetMaterialsBySessionAsync(int sessionId);
    }
}
