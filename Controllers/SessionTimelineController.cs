using LMS.API.DTOs.LiveSession;
using LMS.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SessionTimelineController : ControllerBase
{
    private readonly ILiveSessionService _service;

    public SessionTimelineController(ILiveSessionService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetTimeline([FromQuery] SessionTimelineFilterDto filter)
    {
        var result = await _service.GetTimelineAsync(filter);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpGet("paginated")]
    public async Task<IActionResult> GetPaginated([FromQuery] PaginatedSessionsRequestDto request)
    {
        var result = await _service.GetPaginatedAsync(request);
        return Ok(result);
    }
}
