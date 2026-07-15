using System.Security.Claims;
using LMS.API.DTOs.Assignment;
using LMS.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Student")]
public class AssignmentController : ControllerBase
{
    private readonly IAssignmentService _service;

    public AssignmentController(IAssignmentService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAssignments([FromQuery] AssignmentFilterDto filter)
    {
        var errorResult = GetUserId(out var userId);
        if (errorResult != null) return errorResult;
        var result = await _service.GetAssignmentsAsync(userId, filter);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAssignmentDetails(int id)
    {
        var errorResult = GetUserId(out var userId);
        if (errorResult != null) return errorResult;
        var result = await _service.GetAssignmentDetailsAsync(id, userId);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpPost("submit")]
    public async Task<IActionResult> SubmitAssignment([FromForm] SubmitAssignmentRequestDto dto)
    {
        try
        {
            var errorResult = GetUserId(out var userId);
            if (errorResult != null) return errorResult;
            var result = await _service.SubmitAssignmentAsync(userId, dto);
            if (!result) return BadRequest(new { message = "Could not submit assignment" });
            return Ok(new { message = "Assignment submitted successfully" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    private IActionResult GetUserId(out int userId)
    {
        if (!int.TryParse(User.FindFirstValue("UserId"), out userId))
            return Unauthorized(new { message = "User not authenticated" });
        return null!;
    }
}
