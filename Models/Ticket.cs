using LMS.API.Enums.Ticket;

namespace LMS.API.Models;

public class Ticket
{
    public int Id { get; set; }
    public int StudentId { get; set; } // Foreign key to Students
    public string Title { get; set; } //(عنوان الشكوى - مثل: "مشكلة في رفع واجب السي شارب)
    public string Description { get; set; } //(وصف الشكوى - مثل: "لا أستطيع رفع واجب السي شارب على المنصة")
    public TicketCategory Category { get; set; } //(فئة الشكوى - مثل: Technical, Academic, Financial)
    public TicketPriority Priority { get; set; } //(أولوية الشكوى - مثل: Low, Medium, High)
    public TicketStatus Status { get; set; } //(حالة الشكوى - مثل: Open, InProgress, Resolved, Closed)
    public DateTime CreatedAt { get; set; } //(تاريخ ووقت إنشاء الشكوى)
    public DateTime? UpdatedAt { get; set; } //(تاريخ ووقت آخر تحديث للشكوى)

    //Relationships
    public User Student { get; set; } // علاقة Many-to-One مع Students
    public ICollection<TicketReply> TicketReplies { get; set; } = new List<TicketReply>(); // علاقة One-to-Many مع TicketReply
}
