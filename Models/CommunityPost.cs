namespace LMS.API.Models;

public class CommunityPost
{
    public int Id { get; set; }
    public int UserId { get; set; } // Foreign key to Users-صاحب البوست سواء طالب او مدرس
    public string Content { get; set; } //(محتوى البوست - مثل: "هل يمكن لأحد مساعدتي في فهم واجب السي شارب؟")
    public DateTime CreatedAt { get; set; } //(تاريخ ووقت إنشاء البوست)

    //Relationships
    public User User { get; set; } // علاقة Many-to-One مع Users(طالب او مدرس)
}
