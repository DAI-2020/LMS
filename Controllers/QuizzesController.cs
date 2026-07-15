using System.Security.Claims;
using LMS.API.DTOs.Quiz;
using LMS.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuizzesController : ControllerBase
{
    private readonly IQuizService _quizService;
    private readonly ITaskAndPerformanceService _performanceService;

    public QuizzesController(
        IQuizService quizService,
        ITaskAndPerformanceService performanceService)
    {
        _quizService = quizService;
        _performanceService = performanceService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var quiz = await _quizService.GetByIdAsync(id);
        if (quiz is null) return NotFound();
        return Ok(quiz);
    }

    [HttpGet("growth-areas")]
    public async Task<IActionResult> GetGrowthAreas([FromQuery] int courseId)
    {
        var studentId = GetUserId();
        var result = await _performanceService.GetGrowthAreasAsync(studentId, courseId);
        return Ok(result);
    }

    [HttpGet("available/{courseId}")]
    public async Task<IActionResult> GetAvailableQuizzes(int courseId)
    {
        var quizzes = await _quizService.GetByCourseIdAsync(courseId);
        return Ok(quizzes);
    }

    [HttpGet("student/{studentId}")]
    public async Task<IActionResult> GetByStudentId(int studentId)
    {
        var quizzes = await _quizService.GetByStudentIdAsync(studentId);
        return Ok(quizzes);
    }

    [HttpGet("student/{studentId}/course/{courseId}")]
    public async Task<IActionResult> GetByStudentAndCourse(int studentId, int courseId)
    {
        var quizzes = await _quizService.GetByStudentAndCourseAsync(studentId, courseId);
        return Ok(quizzes);
    }

    [Authorize]
    [HttpPost("submit")]
    public async Task<IActionResult> SubmitQuizAnswers([FromBody] CreateQuizDto dto)
    {
        dto.StudentId = GetUserId();
        var created = await _quizService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [Authorize(Roles = "Admin,Instructor")]
    [HttpPost]
    public async Task<IActionResult> CreateQuiz([FromBody] CreateQuizDto dto)
    {
        try
        {
            var created = await _quizService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize(Roles = "Admin,Instructor")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateQuiz(int id, [FromBody] UpdateQuizDto dto)
    {
        var result = await _quizService.UpdateAsync(id, dto);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteQuiz(int id)
    {
        var result = await _quizService.DeleteAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }

    private int GetUserId()
    {
        var claim = User.FindFirstValue("UserId");
        if (claim == null || !int.TryParse(claim, out var id))
            throw new UnauthorizedAccessException("User ID claim not found.");
        return id;
    }
}
