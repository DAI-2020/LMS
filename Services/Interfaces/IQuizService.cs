using LMS.API.DTOs.Quiz;

namespace LMS.API.Services.Interfaces;

public interface IQuizService
{
    Task<QuizResponseDto?> GetByIdAsync(int id);
    Task<IEnumerable<QuizResponseDto>> GetByStudentIdAsync(int studentId);
    Task<IEnumerable<QuizResponseDto>> GetByCourseIdAsync(int courseId);
    Task<IEnumerable<QuizResponseDto>> GetByStudentAndCourseAsync(int studentId, int courseId);
    Task<QuizResponseDto> CreateAsync(CreateQuizDto dto);
    Task<QuizResponseDto?> UpdateAsync(int id, UpdateQuizDto dto);
    Task<bool> DeleteAsync(int id);
}
