namespace LMS.API.DTOs.Auth
{
    public class AuthResponseDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();
        public string Token { get; set; } = string.Empty;
        public DateTime TokenExpiry { get; set; }
    }
}
