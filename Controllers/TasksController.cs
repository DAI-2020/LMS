using System.Security.Claims;
using LMS.API.DTOs.Task;
using LMS.API.DTOs.Submission;
using LMS.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly IInstructorTaskService _instructorTaskService;
    private readonly IStudentTaskService _studentTaskService;
    private readonly ITaskAndPerformanceService _performanceService;

    public TasksController(
        IInstructorTaskService instructorTaskService,
        IStudentTaskService studentTaskService,
        ITaskAndPerformanceService performanceService)
    {
        _instructorTaskService = instructorTaskService;
        _studentTaskService = studentTaskService;
        _performanceService = performanceService;
    }

    [Authorize]
    [HttpGet("summary")]
    public async Task<IActionResult> GetTasksSummary([FromQuery] int courseId)
    {
        var studentId = GetUserId();
        var summary = await _performanceService.GetTasksSummaryAsync(studentId, courseId);
        return Ok(summary);
    }

    [Authorize(Roles = "Admin,Instructor")]
    [HttpGet("available/{courseId}")]
    public async Task<IActionResult> GetAvailableTasks(int courseId)
    {
        var tasks = await _instructorTaskService.GetByCourseIdAsync(courseId);
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTaskById(int id)
    {
        var task = await _instructorTaskService.GetByIdAsync(id);
        if (task is null) return NotFound();
        return Ok(task);
    }

    [Authorize(Roles = "Student")]
    [HttpPost("submit")]
    public async Task<IActionResult> SubmitHomework([FromBody] CreateSubmissionDto dto)
    {
        dto.StudentId = GetUserId();
        var result = await _studentTaskService.SubmitHomeworkAsync(dto);
        if (result is null)
            return BadRequest(new { message = "Already submitted or task not found" });
        return CreatedAtAction(nameof(GetMySubmissions), result);
    }

    [Authorize]
    [HttpGet("submissions/my")]
    public async Task<IActionResult> GetMySubmissions()
    {
        var userId = GetUserId();
        var submissions = await _studentTaskService.GetSubmissionsByStudentAsync(userId);
        return Ok(submissions);
    }

    [Authorize(Roles = "Admin,Instructor")]
    [HttpPost("submissions/{submissionId}/grade")]
    public async Task<IActionResult> GradeHomework(int submissionId, [FromBody] UpdateSubmission dto)
    {
        var result = await _studentTaskService.GradeHomeworkAsync(submissionId, dto);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [Authorize(Roles = "Admin,Instructor")]
    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto dto)
    {
        try
        {
            var created = await _instructorTaskService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetTaskById), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize(Roles = "Admin,Instructor")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskDto dto)
    {
        var updated = await _instructorTaskService.UpdateAsync(id, dto);
        if (updated is null) return NotFound();
        return Ok(updated);
    }

    [Authorize(Roles = "Admin,Instructor")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var result = await _instructorTaskService.DeleteAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }

    private int GetUserId() => int.Parse(User.FindFirstValue("UserId")!);
}
