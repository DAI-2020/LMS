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
        var userId = GetUserId();
        var prefs = await _service.GetByUserIdAsync(userId);
        if (prefs is null) return NotFound();
        return Ok(prefs);
    }

    [HttpPut]
    public async Task<IActionResult> UpdatePreferences([FromBody] UpdateNotificationPreferencesDto dto)
    {
        var userId = GetUserId();
        var result = await _service.UpdateAsync(userId, dto);
        if (result is null) return NotFound();
        return Ok(result);
    }

    private int GetUserId()
    {
        var claim = User.FindFirstValue("UserId");
        if (claim == null || !int.TryParse(claim, out var id))
            throw new UnauthorizedAccessException("User ID claim not found.");
        return id;
    }
}
