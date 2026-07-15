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
        var userId = GetUserId();
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
        var userId = GetUserId();
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
        var userId = GetUserId();
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
        var userId = GetUserId();
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

    private int GetUserId()
    {
        var claim = User.FindFirstValue("UserId");
        if (claim == null || !int.TryParse(claim, out var id))
            throw new UnauthorizedAccessException("User ID claim not found.");
        return id;
    }
}
