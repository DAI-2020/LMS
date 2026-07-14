using System.Security.Claims;
using LMS.API.DTOs.Admin;
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
    private readonly IAdminService _adminService;

    public AuthController(
        IAuthService authService,
        IProfileAndSecurityService profileService,
        IAdminService adminService)
    {
        _authService = authService;
        _profileService = profileService;
        _adminService = adminService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        try
        {
            var result = await _authService.LoginAsync(dto);
            if (!result.IsSuccess)
                return Unauthorized(new { message = "Invalid email or password", debug = result.ErrorDetail });

            return Ok(result.Response);
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An error occurred during login" });
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
        catch (Exception)
        {
            return StatusCode(500, new { message = "An error occurred during registration" });
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

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
    {
        var result = await _authService.ForgotPasswordAsync(dto);
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("create-user")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserWithRoleDto dto)
    {
        var result = await _adminService.CreateUserWithRoleAsync(dto);
        if (result is null)
            return BadRequest(new { message = "User already exists or role is invalid." });

        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("update-user")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto dto)
    {
        var result = await _adminService.UpdateUserAsync(dto);
        if (!result)
            return NotFound(new { message = "User not found or role is invalid." });

        return Ok(new { message = "User updated successfully." });
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("delete-user/{email}")]
    public async Task<IActionResult> DeleteUser(string email)
    {
        var result = await _adminService.DeleteUserAsync(email);
        if (!result)
            return NotFound(new { message = "User not found." });

        return Ok(new { message = "User deleted successfully." });
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("roles")]
    public async Task<IActionResult> GetAllRoles()
    {
        var roles = await _adminService.GetAllRolesAsync();
        return Ok(roles);
    }

    [Authorize]
    [HttpGet("debug-token")]
    public IActionResult DebugToken()
    {
        var claims = User.Claims.Select(c => new { c.Type, c.Value });
        return Ok(new
        {
            IsAuthenticated = User.Identity?.IsAuthenticated,
            Claims = claims
        });
    }
}
