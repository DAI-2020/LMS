using LMS.API.DTOs.LiveSession;
using LMS.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LiveSessionsController : ControllerBase
{
    private readonly ILiveSessionService _service;

    public LiveSessionsController(ILiveSessionService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetFiltered(
        [FromQuery] string? status,
        [FromQuery] string? type,
        [FromQuery] string? mode,
        [FromQuery] int? courseId,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to)
    {
        var sessions = await _service.GetFilteredAsync(status, type, mode, courseId, from, to);
        return Ok(sessions);
    }

    [HttpGet("upcoming")]
    public async Task<IActionResult> GetUpcoming()
    {
        var sessions = await _service.GetUpcomingSessionsAsync();
        return Ok(sessions);
    }

    [HttpGet("today")]
    public async Task<IActionResult> GetToday()
    {
        var sessions = await _service.GetTodaySessionsAsync();
        return Ok(sessions);
    }

    [HttpGet("completed")]
    public async Task<IActionResult> GetCompleted()
    {
        var sessions = await _service.GetCompletedSessionsAsync();
        return Ok(sessions);
    }

    [HttpGet("missed")]
    public async Task<IActionResult> GetMissed()
    {
        var sessions = await _service.GetMissedSessionsAsync();
        return Ok(sessions);
    }

    [HttpGet("recorded")]
    public async Task<IActionResult> GetRecorded()
    {
        var sessions = await _service.GetRecordedSessionsAsync();
        return Ok(sessions);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var session = await _service.GetByIdAsync(id);
        if (session is null) return NotFound();
        return Ok(session);
    }

    [Authorize(Roles = "Admin,Instructor")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLiveSessionDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [Authorize(Roles = "Admin,Instructor")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateLiveSessionDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);
        if (updated is null) return NotFound();
        return Ok(updated);
    }

    [Authorize(Roles = "Admin,Instructor")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}
