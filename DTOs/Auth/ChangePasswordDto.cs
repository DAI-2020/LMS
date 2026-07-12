using System.ComponentModel.DataAnnotations;

namespace LMS.API.DTOs.Auth
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "كلمة المرور القديمة مطلوبة")]
        public string OldPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "كلمة المرور الجديدة مطلوبة")]
        [MinLength(6, ErrorMessage = "كلمة المرور الجديدة يجب أن تكون 6 أحرف على الأقل")]
        [MaxLength(100)]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "تأكيد كلمة المرور الجديدة مطلوب")]
        [Compare("NewPassword", ErrorMessage = "كلمة المرور الجديدة وتأكيدتها غير متطابقتين")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
