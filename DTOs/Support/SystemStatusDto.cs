namespace LMS.API.DTOs.Support;

public class SystemStatusDto
{
    public bool IsOperational { get; set; }
    public string StatusMessage { get; set; } = string.Empty;
}
