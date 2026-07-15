namespace LMS.API.DTOs.Support;

public class SupportTicketResponseDto
{
    public int Id { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string? AttachmentUrl { get; set; }
    public List<SupportTicketReplyDto> Replies { get; set; } = new();
}
