namespace LMS.API.DTOs.Account;

public class AccountDeviceDto
{
    public int SessionId { get; set; }
    public string DeviceName { get; set; } = string.Empty;
    public string LastUsedText { get; set; } = string.Empty;
    public bool IsCurrent { get; set; }
}
