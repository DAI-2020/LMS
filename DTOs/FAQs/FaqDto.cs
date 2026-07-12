namespace LMS.API.DTOs.FAQs
{
    public class FaqDto
    {
        public int Id { get; set; }
        public string Question { get; set; } //(سؤال شائع)
        public string Answer { get; set; } //(إجابة السؤال الشائع)
    }
}
