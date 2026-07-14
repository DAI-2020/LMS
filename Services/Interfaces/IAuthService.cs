using LMS.API.DTOs.Auth;

namespace LMS.API.Services.Interfaces;

public class LoginResult
{
    public AuthResponseDto? Response { get; set; }
    public bool IsSuccess => Response is not null;
    public string? ErrorDetail { get; set; }
}

public interface IAuthService
{
    Task<LoginResult> LoginAsync(LoginDto dto);
    Task<AuthResponseDto?> RegisterAsync(RegisrerDto dto);
    Task<ForgotPasswordResponseDto> ForgotPasswordAsync(ForgotPasswordDto dto);
}
