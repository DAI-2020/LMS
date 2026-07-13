using LMS.API.DTOs.Quiz;
using LMS.API.Models;
using LMS.API.Repositories.Interfaces;
using LMS.API.Services.Interfaces;

namespace LMS.API.Services.Implementations;

public class QuizService : IQuizService
{
    private readonly IUnitOfWork _unitOfWork;

    public QuizService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<QuizResponseDto?> GetByIdAsync(int id)
    {
        var quiz = await _unitOfWork.Quizzes.GetByIdAsync(id);
        return quiz is null ? null : MapToResponse(quiz);
    }

    public async Task<IEnumerable<QuizResponseDto>> GetByStudentIdAsync(int studentId)
    {
        var quizzes = await _unitOfWork.Quizzes.GetByStudentIdAsync(studentId);
        return quizzes.Select(MapToResponse);
    }

    public async Task<IEnumerable<QuizResponseDto>> GetByCourseIdAsync(int courseId)
    {
        var quizzes = await _unitOfWork.Quizzes.GetByCourseIdAsync(courseId);
        return quizzes.Select(MapToResponse);
    }

    public async Task<IEnumerable<QuizResponseDto>> GetByStudentAndCourseAsync(int studentId, int courseId)
    {
        var quizzes = await _unitOfWork.Quizzes.GetByStudentAndCourseAsync(studentId, courseId);
        return quizzes.Select(MapToResponse);
    }

    public async Task<QuizResponseDto> CreateAsync(CreateQuizDto dto)
    {
        var courseExists = await _unitOfWork.Courses.GetByIdAsync(dto.CourseId);
        if (courseExists is null)
            throw new ArgumentException($"Course with Id {dto.CourseId} not found.");

        var topicExists = await _unitOfWork.Topics.GetByIdAsync(dto.TopicId);
        if (topicExists is null)
            throw new ArgumentException($"Topic with Id {dto.TopicId} not found.");

        var studentExists = await _unitOfWork.Users.GetByIdAsync(dto.StudentId);
        if (studentExists is null)
            throw new ArgumentException($"Student with Id {dto.StudentId} not found.");

        var quiz = new Quiz
        {
            CourseId = dto.CourseId,
            TopicId = dto.TopicId,
            StudentId = dto.StudentId,
            Score = dto.Score,
            TakenAt = DateTime.UtcNow
        };

        await _unitOfWork.Quizzes.AddAsync(quiz);
        await _unitOfWork.SaveChangesAsync();

        return MapToResponse(quiz);
    }

    public async Task<QuizResponseDto?> UpdateAsync(int id, UpdateQuizDto dto)
    {
        var quiz = await _unitOfWork.Quizzes.GetByIdAsync(id);
        if (quiz is null) return null;

        if (dto.Score.HasValue) quiz.Score = dto.Score.Value;

        _unitOfWork.Quizzes.Update(quiz);
        await _unitOfWork.SaveChangesAsync();

        return MapToResponse(quiz);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var quiz = await _unitOfWork.Quizzes.GetByIdAsync(id);
        if (quiz is null) return false;

        _unitOfWork.Quizzes.Delete(quiz);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    private static QuizResponseDto MapToResponse(Quiz q)
    {
        return new QuizResponseDto
        {
            Id = q.Id,
            CourseId = q.CourseId,
            TopicId = q.TopicId,
            StudentId = q.StudentId,
            Score = q.Score,
            TakenAt = q.TakenAt
        };
    }
}
