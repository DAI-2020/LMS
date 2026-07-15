using System.Security.Claims;
using LMS.API.DTOs.Security;
using LMS.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SecurityController : ControllerBase
{
    private readonly ISecurityService _service;

    public SecurityController(ISecurityService service)
    {
        _service = service;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        var errorResult = GetUserId(out var userId);
        if (errorResult != null) return errorResult;
        var tokenHash = GetTokenHash();
        var result = await _service.GetSummaryAsync(userId, tokenHash);
        return Ok(result);
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateSettings([FromBody] UpdateSecuritySettingsDto dto)
    {
        var errorResult = GetUserId(out var userId);
        if (errorResult != null) return errorResult;
        var result = await _service.UpdateSettingsAsync(userId, dto);
        if (!result) return BadRequest(new { message = "Current password is incorrect" });
        return Ok(new { message = "Security settings updated" });
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
