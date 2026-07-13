namespace LMS.API.Services.Interfaces
{
    public interface IProfileAndSecurityService
    {
        Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword);
    }
}
