namespace LMS.API.DTOs.Support;

public class ContactChannelDto
{
    public string ChannelType { get; set; } = string.Empty;
    public string ValueText { get; set; } = string.Empty;
    public string ButtonText { get; set; } = string.Empty;
    public string ActionUrl { get; set; } = string.Empty;
}
