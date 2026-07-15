using System.Security.Claims;
using LMS.API.DTOs.Account;
using LMS.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AccountDetailsController : ControllerBase
{
    private readonly IAccountDetailsService _service;

    public AccountDetailsController(IAccountDetailsService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetDetails()
    {
        var errorResult = GetUserId(out var userId);
        if (errorResult != null) return errorResult;
        var result = await _service.GetDetailsAsync(userId);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpGet("edit-devices")]
    public async Task<IActionResult> GetEditDevices()
    {
        var errorResult = GetUserId(out var userId);
        if (errorResult != null) return errorResult;
        var tokenHash = GetTokenHash();
        var result = await _service.GetDevicesAsync(userId, tokenHash);
        return Ok(result);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateDetails([FromForm] UpdateAccountDetailsRequestDto dto)
    {
        try
        {
            var errorResult = GetUserId(out var userId);
            if (errorResult != null) return errorResult;
            var result = await _service.UpdateDetailsAsync(userId, dto);
            if (!result) return BadRequest(new { message = "Could not update details" });
            return Ok(new { message = "Account details updated" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("disconnect-device/{sessionId}")]
    public async Task<IActionResult> DisconnectDevice(int sessionId)
    {
        var errorResult = GetUserId(out var userId);
        if (errorResult != null) return errorResult;
        var result = await _service.DisconnectDeviceAsync(userId, sessionId);
        if (!result) return NotFound();
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
