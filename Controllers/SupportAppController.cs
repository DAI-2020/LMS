using System.Security.Claims;
using LMS.API.DTOs.Support;
using LMS.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SupportAppController : ControllerBase
{
    private readonly ISupportAppService _service;

    public SupportAppController(ISupportAppService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpGet("landing-info")]
    public async Task<IActionResult> GetLandingInfo()
    {
        var userId = GetUserId();
        var result = await _service.GetLandingInfoAsync(userId);
        return Ok(result);
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories()
    {
        var result = await _service.GetCategoriesAsync();
        return Ok(result);
    }

    [HttpGet("system-status")]
    public async Task<IActionResult> GetSystemStatus()
    {
        var result = await _service.GetSystemStatusAsync();
        return Ok(result);
    }

    [Authorize(Roles = "Student")]
    [HttpPost("submit-ticket")]
    public async Task<IActionResult> SubmitTicket([FromForm] SubmitTicketRequestDto dto)
    {
        try
        {
            var userId = GetUserId();
            var result = await _service.SubmitTicketAsync(userId, dto);
            if (result is null) return BadRequest(new { message = "Could not submit ticket" });
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpGet("my-tickets")]
    public async Task<IActionResult> GetMyTickets()
    {
        var userId = GetUserId();
        var result = await _service.GetMyTicketsAsync(userId);
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
