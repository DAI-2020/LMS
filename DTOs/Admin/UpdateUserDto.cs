using System.ComponentModel.DataAnnotations;

namespace LMS.API.DTOs.Admin
{
    public class UpdateUserDto
    {
        [Required]
        [EmailAddress]
        public string CurrentEmail { get; set; }

        [EmailAddress]
        public string? NewEmail { get; set; }

        [MaxLength(100)]
        public string? FullName { get; set; }

        [MinLength(6)]
        [MaxLength(100)]
        public string? NewPassword { get; set; }

        public string? NewRoleName { get; set; }
    }
}
