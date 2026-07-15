using System.Security.Claims;
using LMS.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserDevicesController : ControllerBase
{
    private readonly IUserDeviceService _service;

    public UserDevicesController(IUserDeviceService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetMyDevices()
    {
        var errorResult = GetUserId(out var userId);
        if (errorResult != null) return errorResult;
        var tokenHash = GetTokenHash();
        var devices = await _service.GetUserDevicesAsync(userId, tokenHash);
        return Ok(devices);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DisconnectDevice(int id)
    {
        var errorResult = GetUserId(out var userId);
        if (errorResult != null) return errorResult;
        var result = await _service.DisconnectDeviceAsync(userId, id);
        if (!result) return NotFound(new { message = "Device not found" });
        return NoContent();
    }

    [HttpDelete("clear-all")]
    public async Task<IActionResult> ClearAllOtherDevices()
    {
        var errorResult = GetUserId(out var userId);
        if (errorResult != null) return errorResult;
        await _service.DisconnectAllDevicesAsync(userId);
        return NoContent();
    }

    private IActionResult GetUserId(out int userId)
    {
        if (!int.TryParse(User.FindFirstValue("UserId"), out userId))
            return Unauthorized(new { message = "User not authenticated" });
        return null!;
    }

    private string GetTokenHash() =>
        User.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti) ?? string.Empty;
}
