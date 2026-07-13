using LMS.API.DTOs.Course;
using LMS.API.Models;
using LMS.API.Repositories.Interfaces;
using LMS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS.API.Services.Implementations;

public class CourseService : ICourseService
{
    private readonly IUnitOfWork _unitOfWork;

    public CourseService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CourseResponseDto>> GetAllAsync()
    {
        var courses = await _unitOfWork.Courses.GetAllAsync();
        return courses.Select(MapToResponse);
    }

    public async Task<CourseResponseDto?> GetByIdAsync(int id)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(id);
        return course is null ? null : MapToResponse(course);
    }

    public async Task<IEnumerable<CourseResponseDto>> GetByInstructorIdAsync(int instructorId)
    {
        var courses = await _unitOfWork.Courses.GetAllAsync();
        return courses
            .Where(c => c.InstructorId == instructorId)
            .Select(MapToResponse);
    }

    public async Task<IEnumerable<CourseResponseDto>> GetMyCoursesAsync(int userId, IEnumerable<string> roles)
    {
        var allCourses = await _unitOfWork.Courses.GetAllAsync();

        if (roles.Contains("Instructor") || roles.Contains("Admin"))
        {
            return allCourses
                .Where(c => c.InstructorId == userId)
                .Select(MapToResponse);
        }

        var taskSubmissions = await _unitOfWork.TaskSubmissions.GetAllAsync();
        var quizzes = await _unitOfWork.Quizzes.GetAllAsync();

        var studentCourseIds = taskSubmissions
            .Where(s => s.StudentId == userId)
            .Select(s => s.CourseTask.CourseId)
            .Union(quizzes.Where(q => q.StudentId == userId).Select(q => q.CourseId))
            .Distinct();

        return allCourses
            .Where(c => studentCourseIds.Contains(c.Id))
            .Select(MapToResponse);
    }

    public async Task<CourseResponseDto> CreateAsync(CreateCourse dto)
    {
        var course = new Course
        {
            Title = dto.Title,
            Description = dto.Description,
            InstructorId = dto.InstructorId
        };

        await _unitOfWork.Courses.AddAsync(course);
        await _unitOfWork.SaveChangesAsync();

        return MapToResponse(course);
    }

    public async Task<CourseResponseDto?> UpdateAsync(int id, UpdateCourseDto dto)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(id);
        if (course is null) return null;

        if (dto.Title is not null) course.Title = dto.Title;
        if (dto.Description is not null) course.Description = dto.Description;

        _unitOfWork.Courses.Update(course);
        await _unitOfWork.SaveChangesAsync();

        return MapToResponse(course);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(id);
        if (course is null) return false;

        _unitOfWork.Courses.Delete(course);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    private static CourseResponseDto MapToResponse(Course c)
    {
        return new CourseResponseDto
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description,
            InstructorId = c.InstructorId
        };
    }
}
