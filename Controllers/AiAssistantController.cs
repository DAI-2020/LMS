using System.Security.Claims;
using LMS.API.DTOs.AIAssistantChat;
using LMS.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AiAssistantController : ControllerBase
{
    private readonly IAiAssistantService _service;

    public AiAssistantController(IAiAssistantService service)
    {
        _service = service;
    }

    [HttpPost("chat")]
    public async Task<IActionResult> GeneralChat([FromBody] AiChatRequestDto dto)
    {
        var errorResult = GetUserId(out var userId);
        if (errorResult != null) return errorResult;
        try
        {
            dto.StudentId = userId;
            var result = await _service.ChatAsync(dto);
            if (result is null) return BadRequest(new { message = "Could not process request" });
            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An error occurred while processing your request" });
        }
    }

    [HttpPost("help-me-writing")]
    public async Task<IActionResult> HelpMeWriting([FromBody] HelpMeWritingDto dto)
    {
        var errorResult = GetUserId(out var userId);
        if (errorResult != null) return errorResult;
        try
        {
            var result = await _service.HelpMeWritingAsync(userId, dto.CourseId, dto.Text);
            if (result is null) return BadRequest(new { message = "Could not process request" });
            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An error occurred while processing your request" });
        }
    }

    [HttpPost("study-plan")]
    public async Task<IActionResult> CreateStudyPlan([FromBody] StudyPlanDto dto)
    {
        var errorResult = GetUserId(out var userId);
        if (errorResult != null) return errorResult;
        try
        {
            var result = await _service.CreateStudyPlanAsync(userId, dto.CourseId, dto.AdditionalInfo);
            if (result is null) return BadRequest(new { message = "Could not process request" });
            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An error occurred while processing your request" });
        }
    }

    [HttpPost("summarize")]
    public async Task<IActionResult> SummarizeLesson([FromBody] SummarizeLessonDto dto)
    {
        var errorResult = GetUserId(out var userId);
        if (errorResult != null) return errorResult;
        try
        {
            var result = await _service.SummarizeLessonAsync(userId, dto.CourseId, dto.LessonContent);
            if (result is null) return BadRequest(new { message = "Could not process request" });
            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An error occurred while processing your request" });
        }
    }

    private IActionResult GetUserId(out int userId)
    {
        if (!int.TryParse(User.FindFirstValue("UserId"), out userId))
            return Unauthorized(new { message = "User not authenticated" });
        return null!;
    }
}
