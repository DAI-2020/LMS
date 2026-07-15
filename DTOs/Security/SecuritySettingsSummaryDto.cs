namespace LMS.API.DTOs.Security;

public class SecuritySettingsSummaryDto
{
    public bool IsTwoStepVerificationEnabled { get; set; }
    public string? SecurityQuestion { get; set; }
    public List<ActiveDeviceDto> ActiveDevices { get; set; } = new();
}
