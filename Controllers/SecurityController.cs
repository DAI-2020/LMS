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
        var userId = GetUserId();
        var tokenHash = GetTokenHash();
        var result = await _service.GetSummaryAsync(userId, tokenHash);
        return Ok(result);
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateSettings([FromBody] UpdateSecuritySettingsDto dto)
    {
        var userId = GetUserId();
        var result = await _service.UpdateSettingsAsync(userId, dto);
        if (!result) return BadRequest(new { message = "Current password is incorrect" });
        return Ok(new { message = "Security settings updated" });
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
