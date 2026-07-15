namespace LMS.API.DTOs.Support;

public class QuickActionDto
{
    public string IconKey { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ActionText { get; set; } = string.Empty;
    public string RedirectUrl { get; set; } = string.Empty;
}
