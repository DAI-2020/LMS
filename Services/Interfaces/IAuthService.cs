using LMS.API.DTOs.Auth;

namespace LMS.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> LoginAsync(LoginDto dto);
        Task<AuthResponseDto?> RegisterAsync(RegisrerDto dto);
    }
}
