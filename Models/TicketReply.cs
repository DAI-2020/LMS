namespace LMS.API.Models;

    public class TicketReply
    {
        public int Id { get; set; }
        public int TicketId { get; set; } // Foreign key to Ticket
        public int UserId { get; set; } // Foreign key to Users (could be a student or staff)
        public string Message { get; set; } //(نص الرد على الشكوى)
        public DateTime CreatedAt { get; set; } //(تاريخ ووقت إنشاء الرد)

        //Relationships
        public Ticket Ticket { get; set; } // علاقة Many-to-One مع Ticket
        public User User { get; set; } // علاقة Many-to-One مع Users
    }

