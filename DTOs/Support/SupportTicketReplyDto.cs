namespace LMS.API.DTOs.Support;

public class SupportTicketReplyDto
{
    public int Id { get; set; }
    public string SenderName { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
