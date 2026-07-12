namespace LMS.API.Models
{
    public class FAQ
    {
        public int Id { get; set; }
        public string Question { get; set; } //(سؤال شائع)
        public string Answer { get; set; } //(إجابة السؤال الشائع)
    }
}
