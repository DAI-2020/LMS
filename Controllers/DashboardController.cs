using System.Security.Claims;
using LMS.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("tasks-summary")]
    public async Task<IActionResult> GetTasksSummary()
    {
        var userId = GetUserId();
        var result = await _dashboardService.GetTasksSummaryAsync(userId);
        return Ok(result);
    }

    [HttpGet("attendance-summary")]
    public async Task<IActionResult> GetAttendanceSummary()
    {
        var userId = GetUserId();
        var result = await _dashboardService.GetAttendanceSummaryAsync(userId);
        return Ok(result);
    }

    [HttpGet("active-sessions")]
    public async Task<IActionResult> GetActiveSessions()
    {
        var result = await _dashboardService.GetActiveSessionsAsync();
        return Ok(result);
    }

    [HttpGet("growth-areas")]
    public async Task<IActionResult> GetGrowthAreas()
    {
        var userId = GetUserId();
        var result = await _dashboardService.GetGrowthAreasAsync(userId);
        return Ok(result);
    }

    private int GetUserId() => int.Parse(User.FindFirstValue("UserId")!);
}
