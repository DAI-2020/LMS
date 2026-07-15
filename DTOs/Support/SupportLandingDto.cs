namespace LMS.API.DTOs.Support;

public class SupportLandingDto
{
    public string StudentFirstName { get; set; } = string.Empty;
    public List<QuickActionDto> QuickActions { get; set; } = new();
    public List<ContactChannelDto> ContactChannels { get; set; } = new();
}
