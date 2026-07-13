using LMS.API.Models;

namespace LMS.API.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user, IEnumerable<string> roles);
    }
}
