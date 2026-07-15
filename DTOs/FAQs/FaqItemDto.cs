namespace LMS.API.DTOs.FAQs;

public class FaqItemDto
{
    public int Id { get; set; }
    public int Order { get; set; }
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
}
