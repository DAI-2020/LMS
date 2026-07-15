using System.Security.Claims;
using LMS.API.DTOs.Attendance;
using LMS.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AttendanceController : ControllerBase
{
    private readonly IAttendanceService _attendanceService;
    private readonly IAttendanceSummaryService _summaryService;

    public AttendanceController(
        IAttendanceService attendanceService,
        IAttendanceSummaryService summaryService)
    {
        _attendanceService = attendanceService;
        _summaryService = summaryService;
    }

    [Authorize(Roles = "Admin,Instructor")]
    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary(
        [FromQuery] int studentId,
        [FromQuery] string? sessionType,
        [FromQuery] string? timeRange)
    {
        var filter = new AttendanceFilterDto
        {
            SessionType = sessionType,
            TimeRange = timeRange
        };
        var result = await _summaryService.GetAttendanceSummaryAsync(studentId, filter);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("cumulative")]
    public async Task<IActionResult> GetCumulativePercentage(
        [FromQuery] int studentId,
        [FromQuery] string? sessionType)
    {
        var percentage = await _summaryService.GetCumulativeAttendancePercentageAsync(studentId, sessionType);
        return Ok(new { studentId, sessionType, percentage });
    }

    [Authorize]
    [HttpPost("join")]
    public async Task<IActionResult> JoinSession([FromBody] JoinSessionDto dto)
    {
        var errorResult = GetUserId(out var userId);
        if (errorResult != null) return errorResult;
        dto.StudentId = userId;
        var result = await _attendanceService.JoinSessionAsync(dto);
        if (!result) return BadRequest(new { message = "Already joined or session not found" });
        return Ok(new { message = "Joined successfully" });
    }

    [Authorize]
    [HttpPost("leave")]
    public async Task<IActionResult> LeaveSession([FromBody] LeaveSessionDto dto)
    {
        var errorResult = GetUserId(out var userId);
        if (errorResult != null) return errorResult;
        dto.StudentId = userId;
        var result = await _attendanceService.LeaveSessionAsync(dto);
        if (!result) return BadRequest(new { message = "Not joined or already left" });
        return Ok(new { message = "Left successfully" });
    }

    [Authorize(Roles = "Admin,Instructor")]
    [HttpGet("student/{studentId}")]
    public async Task<IActionResult> GetStudentAttendance(int studentId)
    {
        var summary = await _attendanceService.GetStudentSummaryAsync(studentId);
        if (summary is null) return NotFound();
        return Ok(summary);
    }

    [Authorize(Roles = "Admin,Instructor")]
    [HttpGet("session/{sessionId}")]
    public async Task<IActionResult> GetSessionAttendance(int sessionId)
    {
        var logs = await _attendanceService.GetSessionAttendanceAsync(sessionId);
        return Ok(logs);
    }

    private IActionResult GetUserId(out int userId)
    {
        if (!int.TryParse(User.FindFirstValue("UserId"), out userId))
            return Unauthorized(new { message = "User not authenticated" });
        return null!;
    }
}
