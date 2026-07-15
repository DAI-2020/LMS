using LMS.API.DTOs.FAQs;
using LMS.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FaqController : ControllerBase
{
    private readonly IFaqAppService _service;

    public FaqController(IFaqAppService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetFaqs()
    {
        var result = await _service.GetFaqPageAsync();
        return Ok(result);
    }
}
