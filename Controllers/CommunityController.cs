using System.Security.Claims;
using LMS.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommunityController : ControllerBase
{
    private readonly ICommunityService _service;

    public CommunityController(ICommunityService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetFeed()
    {
        var posts = await _service.GetAllAsync();
        return Ok(posts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var post = await _service.GetByIdAsync(id);
        if (post is null) return NotFound();
        return Ok(post);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] string content)
    {
        var userId = GetUserId();
        var created = await _service.CreateAsync(userId, content);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        var userId = GetUserId();
        var isAdmin = User.IsInRole("Admin");
        var posts = await _service.GetAllAsync();
        var post = posts.FirstOrDefault(p => p.Id == id);

        if (post is null) return NotFound();
        if (post.UserId != userId && !isAdmin)
            return Forbid();

        var result = await _service.DeleteAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }

    private int GetUserId() => int.Parse(User.FindFirstValue("UserId")!);
}
