using System.ComponentModel.DataAnnotations;

namespace LMS.API.DTOs.Ticket
{
    public class CreateTicketDto
    {
        public int StudentId { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string Category { get; set; } = string.Empty;
        [Required]
        public string Priority { get; set; } = string.Empty;
    }
}
