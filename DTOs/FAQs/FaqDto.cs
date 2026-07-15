namespace LMS.API.DTOs.FAQs
{
    public class FaqDto
    {
        public int Id { get; set; }
        public string Question { get; set; } = string.Empty; //(سؤال شائع)
        public string Answer { get; set; } = string.Empty; //(إجابة السؤال الشائع)
    }
}
