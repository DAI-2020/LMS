using System.ComponentModel.DataAnnotations;

namespace LMS.API.DTOs.FAQs
{
    public class UpsertFaqDto
    {
        [Required(ErrorMessage = "السؤال مطلوب")]
        public string Question { get; set; }

        [Required(ErrorMessage = "الإجابة مطلوبة")]
        public string Answer { get; set; }
    }
}
