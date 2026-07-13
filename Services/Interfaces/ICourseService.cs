using LMS.API.DTOs.Course;

namespace LMS.API.Services.Interfaces;

public interface ICourseService
{
    Task<IEnumerable<CourseResponseDto>> GetAllAsync();
    Task<CourseResponseDto?> GetByIdAsync(int id);
    Task<IEnumerable<CourseResponseDto>> GetByInstructorIdAsync(int instructorId);
    Task<IEnumerable<CourseResponseDto>> GetMyCoursesAsync(int userId, IEnumerable<string> roles);
    Task<CourseResponseDto> CreateAsync(CreateCourse dto);
    Task<CourseResponseDto?> UpdateAsync(int id, UpdateCourseDto dto);
    Task<bool> DeleteAsync(int id);
}
