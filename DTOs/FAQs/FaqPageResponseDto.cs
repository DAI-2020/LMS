namespace LMS.API.DTOs.FAQs;

public class FaqPageResponseDto
{
    public string Title { get; set; } = "Frequently Asked Questions";
    public string Subtitle { get; set; } = "Find answers to common questions";
    public List<FaqItemDto> Faqs { get; set; } = new();
}
