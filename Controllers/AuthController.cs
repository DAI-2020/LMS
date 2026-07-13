using System.Security.Claims;
using LMS.API.DTOs.Auth;
using LMS.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IProfileAndSecurityService _profileService;

    public AuthController(IAuthService authService, IProfileAndSecurityService profileService)
    {
        _authService = authService;
        _profileService = profileService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        try
        {
            var result = await _authService.LoginAsync(dto);
            if (result is null)
                return Unauthorized(new { message = "Invalid email or password" });

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred during login", detail = ex.Message });
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisrerDto dto)
    {
        try
        {
            var result = await _authService.RegisterAsync(dto);
            if (result is null)
                return Conflict(new { message = "Email already exists" });

            return Created("", result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred during registration", detail = ex.Message });
        }
    }

    [Authorize]
    [HttpGet("profile")]
    public IActionResult GetProfile()
    {
        var userId = User.FindFirstValue("UserId");
        var email = User.FindFirstValue(ClaimTypes.Email);
        var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

        return Ok(new
        {
            UserId = userId,
            Email = email,
            Roles = roles
        });
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        var userId = int.Parse(User.FindFirstValue("UserId")!);
        var result = await _profileService.ChangePasswordAsync(userId, dto.OldPassword, dto.NewPassword);
        if (!result) return BadRequest(new { message = "Old password is incorrect" });
        return Ok(new { message = "Password changed successfully" });
    }
}
