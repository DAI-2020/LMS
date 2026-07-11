namespace LMS.API.DTOs.Ticket
{
    public class TicketReplyDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
