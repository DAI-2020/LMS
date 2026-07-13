using System.Security.Claims;
using LMS.API.DTOs.FAQs;
using LMS.API.DTOs.Ticket;
using LMS.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SupportController : ControllerBase
{
    private readonly ITicketService _ticketService;
    private readonly IFaqService _faqService;

    public SupportController(ITicketService ticketService, IFaqService faqService)
    {
        _ticketService = ticketService;
        _faqService = faqService;
    }

    [HttpGet("faq")]
    public async Task<IActionResult> GetFaqs()
    {
        var faqs = await _faqService.GetAllFaqsAsync();
        return Ok(faqs);
    }

    [HttpGet("faq/{id}")]
    public async Task<IActionResult> GetFaqById(int id)
    {
        var faq = await _faqService.GetFaqByIdAsync(id);
        if (faq is null) return NotFound();
        return Ok(faq);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("faq")]
    public async Task<IActionResult> CreateFaq([FromBody] UpsertFaqDto dto)
    {
        var created = await _faqService.CreateFaqAsync(dto);
        return CreatedAtAction(nameof(GetFaqById), new { id = created.Id }, created);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("faq/{id}")]
    public async Task<IActionResult> UpdateFaq(int id, [FromBody] UpsertFaqDto dto)
    {
        var result = await _faqService.UpdateFaqAsync(id, dto);
        if (!result) return NotFound();
        return Ok(new { message = "FAQ updated successfully" });
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("faq/{id}")]
    public async Task<IActionResult> DeleteFaq(int id)
    {
        var result = await _faqService.DeleteFaqAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }

    [Authorize(Roles = "Student")]
    [HttpPost("tickets")]
    public async Task<IActionResult> CreateTicket([FromBody] CreateTicketDto dto)
    {
        dto.StudentId = GetUserId();
        var created = await _ticketService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetTicketById), new { id = created.Id }, created);
    }

    [Authorize]
    [HttpGet("tickets/my")]
    public async Task<IActionResult> GetMyTickets()
    {
        var userId = GetUserId();
        var tickets = await _ticketService.GetMyTicketsAsync(userId);
        return Ok(tickets);
    }

    [Authorize]
    [HttpGet("tickets/{id}")]
    public async Task<IActionResult> GetTicketById(int id)
    {
        var ticket = await _ticketService.GetByIdAsync(id);
        if (ticket is null) return NotFound();
        return Ok(ticket);
    }

    [Authorize]
    [HttpGet("tickets/{id}/replies")]
    public async Task<IActionResult> GetTicketReplies(int id)
    {
        var ticket = await _ticketService.GetByIdAsync(id);
        if (ticket is null) return NotFound();
        return Ok(ticket.Replies);
    }

    [Authorize]
    [HttpPost("tickets/{id}/reply")]
    public async Task<IActionResult> AddReply(int id, [FromBody] string message)
    {
        var userId = GetUserId();
        var reply = await _ticketService.AddReplyAsync(id, userId, message);
        return Ok(reply);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("tickets")]
    public async Task<IActionResult> GetAllOpenTickets(
        [FromQuery] string? status,
        [FromQuery] string? category,
        [FromQuery] string? priority,
        [FromQuery] int? studentId)
    {
        var tickets = await _ticketService.GetFilteredAsync(status, category, priority, studentId);
        return Ok(tickets);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("tickets/{id}/status")]
    public async Task<IActionResult> UpdateTicketStatus(int id, [FromBody] string status)
    {
        var result = await _ticketService.UpdateStatusAsync(id, status);
        if (result is null) return NotFound();
        return Ok(result);
    }

    private int GetUserId() => int.Parse(User.FindFirstValue("UserId")!);
}
