namespace LMS.API.DTOs.Security;

public class ActiveDeviceDto
{
    public int SessionId { get; set; }
    public string DeviceType { get; set; } = string.Empty;
    public string BrowserAndOs { get; set; } = string.Empty;
    public DateTime LastActiveDateTime { get; set; }
    public string LastActiveText { get; set; } = string.Empty;
    public bool IsCurrentSession { get; set; }
}
