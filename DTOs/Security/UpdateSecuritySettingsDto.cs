using System.ComponentModel.DataAnnotations;

namespace LMS.API.DTOs.Security;

public class UpdateSecuritySettingsDto
{
    [Required]
    public string CurrentPassword { get; set; } = string.Empty;

    public string? NewPassword { get; set; }

    public bool? EnableTwoStepVerification { get; set; }
}
