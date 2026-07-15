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
        var userId = GetUserId();
        var result = await _service.GetDetailsAsync(userId);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpGet("edit-devices")]
    public async Task<IActionResult> GetEditDevices()
    {
        var userId = GetUserId();
        var tokenHash = GetTokenHash();
        var result = await _service.GetDevicesAsync(userId, tokenHash);
        return Ok(result);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateDetails([FromForm] UpdateAccountDetailsRequestDto dto)
    {
        try
        {
            var userId = GetUserId();
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
        var userId = GetUserId();
        var result = await _service.DisconnectDeviceAsync(userId, sessionId);
        if (!result) return NotFound();
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
