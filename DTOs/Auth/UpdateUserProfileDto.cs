using System.ComponentModel.DataAnnotations;

namespace LMS.API.DTOs.Auth
{
    public class UpdateUserProfileDto
    {
        [MaxLength(100)]
        public string? FullName { get; set; }

        [EmailAddress]
        [MaxLength(150)]
        public string? Email { get; set; }

        [Phone]
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        public string? Gender { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        [MaxLength(250)]
        public string? Address { get; set; }
    }
}
