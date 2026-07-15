using System.Security.Claims;
using LMS.API.DTOs.NotificationPreferences;
using LMS.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationPreferencesController : ControllerBase
{
    private readonly INotificationPreferenceService _service;

    public NotificationPreferencesController(INotificationPreferenceService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetMyPreferences()
    {
        var errorResult = GetUserId(out var userId);
        if (errorResult != null) return errorResult;
        var prefs = await _service.GetByUserIdAsync(userId);
        if (prefs is null) return NotFound();
        return Ok(prefs);
    }

    [HttpPut]
    public async Task<IActionResult> UpdatePreferences([FromBody] UpdateNotificationPreferencesDto dto)
    {
        var errorResult = GetUserId(out var userId);
        if (errorResult != null) return errorResult;
        var result = await _service.UpdateAsync(userId, dto);
        if (result is null) return NotFound();
        return Ok(result);
    }

    private IActionResult GetUserId(out int userId)
    {
        if (!int.TryParse(User.FindFirstValue("UserId"), out userId))
            return Unauthorized(new { message = "User not authenticated" });
        return null!;
    }
}
