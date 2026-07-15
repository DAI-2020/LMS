using System.ComponentModel.DataAnnotations;

namespace LMS.API.DTOs.Support;

public class SubmitTicketRequestDto
{
    [Required]
    public string Subject { get; set; } = string.Empty;

    [Required]
    public int CategoryId { get; set; }

    [Required]
    public string Description { get; set; } = string.Empty;

    public IFormFile? Attachment { get; set; }
}
