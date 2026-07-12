namespace LMS.API.Models
{
    public class UserDevice
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string DeviceName { get; set; } = string.Empty; // iPhone 14 Pro مثلاً
        public string? ClientInfo { get; set; } // Mozilla Firefox مثلاً
        public string RefreshTokenHash { get; set; } = string.Empty; // لزوم الـ Disconnect والـ Auth
        public DateTime LastUsed { get; set; } = DateTime.UtcNow;

        // Navigation Property
        public User User { get; set; } = null!;
    }
}
