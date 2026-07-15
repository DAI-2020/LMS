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
        var userId = GetUserId();
        var tokenHash = GetTokenHash();
        var devices = await _service.GetUserDevicesAsync(userId, tokenHash);
        return Ok(devices);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DisconnectDevice(int id)
    {
        var userId = GetUserId();
        var result = await _service.DisconnectDeviceAsync(userId, id);
        if (!result) return NotFound(new { message = "Device not found" });
        return NoContent();
    }

    [HttpDelete("clear-all")]
    public async Task<IActionResult> ClearAllOtherDevices()
    {
        var userId = GetUserId();
        await _service.DisconnectAllDevicesAsync(userId);
        return NoContent();
    }

    private int GetUserId()
    {
        var claim = User.FindFirstValue("UserId");
        if (claim == null || !int.TryParse(claim, out var id))
            throw new UnauthorizedAccessException("User ID claim not found.");
        return id;
    }

    private string GetTokenHash() =>
        User.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti) ?? string.Empty;
}
