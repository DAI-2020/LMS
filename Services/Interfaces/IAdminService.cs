using LMS.API.DTOs.Admin;

namespace LMS.API.Services.Interfaces;

public interface IAdminService
{
    Task<AdminUserResponseDto?> CreateUserWithRoleAsync(CreateUserWithRoleDto dto);
    Task<bool> UpdateUserAsync(UpdateUserDto dto);
    Task<bool> DeleteUserAsync(string email);
    Task<List<RoleResponseDto>> GetAllRolesAsync();
}
