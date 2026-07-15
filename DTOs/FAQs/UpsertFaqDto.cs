using System.ComponentModel.DataAnnotations;

namespace LMS.API.DTOs.FAQs
{
    public class UpsertFaqDto
    {
        [Required(ErrorMessage = "السؤال مطلوب")]
        public string Question { get; set; } = string.Empty;

        [Required(ErrorMessage = "الإجابة مطلوبة")]
        public string Answer { get; set; } = string.Empty;
    }
}
