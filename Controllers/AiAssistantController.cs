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
        try
        {
            dto.StudentId = GetUserId();
            var result = await _service.ChatAsync(dto);
            if (result is null) return BadRequest(new { message = "Could not process request" });
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while processing your request", detail = ex.Message });
        }
    }

    [HttpPost("help-me-writing")]
    public async Task<IActionResult> HelpMeWriting([FromBody] HelpMeWritingDto dto)
    {
        try
        {
            var userId = GetUserId();
            var result = await _service.HelpMeWritingAsync(userId, dto.CourseId, dto.Text);
            if (result is null) return BadRequest(new { message = "Could not process request" });
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while processing your request", detail = ex.Message });
        }
    }

    [HttpPost("study-plan")]
    public async Task<IActionResult> CreateStudyPlan([FromBody] StudyPlanDto dto)
    {
        try
        {
            var userId = GetUserId();
            var result = await _service.CreateStudyPlanAsync(userId, dto.CourseId, dto.AdditionalInfo);
            if (result is null) return BadRequest(new { message = "Could not process request" });
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while processing your request", detail = ex.Message });
        }
    }

    [HttpPost("summarize")]
    public async Task<IActionResult> SummarizeLesson([FromBody] SummarizeLessonDto dto)
    {
        try
        {
            var userId = GetUserId();
            var result = await _service.SummarizeLessonAsync(userId, dto.CourseId, dto.LessonContent);
            if (result is null) return BadRequest(new { message = "Could not process request" });
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while processing your request", detail = ex.Message });
        }
    }

    private int GetUserId() => int.Parse(User.FindFirstValue("UserId")!);
}
