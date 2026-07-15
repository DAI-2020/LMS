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
        var userId = GetUserId();
        var result = await _service.GetAssignmentsAsync(userId, filter);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAssignmentDetails(int id)
    {
        var userId = GetUserId();
        var result = await _service.GetAssignmentDetailsAsync(id, userId);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpPost("submit")]
    public async Task<IActionResult> SubmitAssignment([FromForm] SubmitAssignmentRequestDto dto)
    {
        try
        {
            var userId = GetUserId();
            var result = await _service.SubmitAssignmentAsync(userId, dto);
            if (!result) return BadRequest(new { message = "Could not submit assignment" });
            return Ok(new { message = "Assignment submitted successfully" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
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
