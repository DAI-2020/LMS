using System.Security.Claims;
using LMS.API.DTOs.GraduationProject;
using LMS.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GraduationProjectController : ControllerBase
{
    private readonly IGraduationProjectService _service;

    public GraduationProjectController(IGraduationProjectService service)
    {
        _service = service;
    }

    [Authorize(Roles = "Student")]
    [HttpGet("upload-metadata")]
    public async Task<IActionResult> GetUploadMetadata()
    {
        var result = await _service.GetUploadMetadataAsync();
        return Ok(result);
    }

    [Authorize(Roles = "Student")]
    [HttpPost("submit")]
    public async Task<IActionResult> SubmitProject([FromForm] SubmitGraduationProjectDto dto)
    {
        try
        {
            var errorResult = GetUserId(out var userId);
            if (errorResult != null) return errorResult;
            dto.StudentId = userId;
            var result = await _service.SubmitProjectAsync(dto);
            if (result is null)
                return BadRequest(new { message = "Already submitted" });

            return CreatedAtAction(nameof(GetMySubmissionStatus), result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize(Roles = "Student")]
    [HttpGet("my-submission")]
    public async Task<IActionResult> GetMySubmissionStatus()
    {
        var errorResult = GetUserId(out var userId);
        if (errorResult != null) return errorResult;
        var result = await _service.GetByStudentIdAsync(userId);
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAllSubmissions()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateProjectStatus(int id, [FromBody] string status)
    {
        try
        {
            var result = await _service.UpdateStatusAsync(id, status);
            if (result is null) return NotFound();
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }

    private IActionResult GetUserId(out int userId)
    {
        if (!int.TryParse(User.FindFirstValue("UserId"), out userId))
            return Unauthorized(new { message = "User not authenticated" });
        return null!;
    }
}
