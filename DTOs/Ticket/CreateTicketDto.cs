namespace LMS.API.DTOs.Ticket
{
    public class CreateTicketDto
    {
        public int StudentId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Priority { get; set; }
    }
}
