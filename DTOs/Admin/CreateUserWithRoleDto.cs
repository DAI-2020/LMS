using System.ComponentModel.DataAnnotations;

namespace LMS.API.DTOs.Admin
{
    public class CreateUserWithRoleDto
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(100)]
        public string Password { get; set; }

        [Required]
        public string RoleName { get; set; }
    }
}
