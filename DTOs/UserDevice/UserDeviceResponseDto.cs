namespace LMS.API.DTOs.UserDevice
{
    public class UserDeviceResponseDto
    {
        public int Id { get; set; }
        public string DeviceName { get; set; } = string.Empty;   // مثل: iPhone 14 Pro
        public string? ClientInfo { get; set; }                  // مثل: Chrome / Mozilla Firefox
        public DateTime LastUsed { get; set; }
        public bool IsCurrentDevice { get; set; }                // حقل ذكي بنحسبه عشان الفرونت يعلم على الجهاز اللي الطالب فاتح منه دلوقتي بكلمة (This Device)
    }
}
