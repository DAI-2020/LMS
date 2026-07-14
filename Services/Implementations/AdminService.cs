using LMS.API.Data;
using LMS.API.DTOs.Admin;
using LMS.API.Enums.User;
using LMS.API.Models;
using LMS.API.Repositories.Interfaces;
using LMS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS.API.Services.Implementations;

public class AdminService : IAdminService
{
    private readonly LMSDbContext _dbContext;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AdminService(
        LMSDbContext dbContext,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _dbContext = dbContext;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<AdminUserResponseDto?> CreateUserWithRoleAsync(CreateUserWithRoleDto dto)
    {
        var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
        if (existingUser is not null)
            return null;

        var role = await _dbContext.Roles
            .FirstOrDefaultAsync(r => r.Name == dto.RoleName);
        if (role is null)
            return null;

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            CreatedAt = DateTime.UtcNow,
            Gender = Gender.Male
        };

        await _userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        _dbContext.UserRoles.Add(new UserRole
        {
            UserId = user.Id,
            RoleId = role.Id
        });
        await _unitOfWork.SaveChangesAsync();

        return new AdminUserResponseDto
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            RoleName = role.Name,
            CreatedAt = user.CreatedAt
        };
    }

    public async Task<bool> UpdateUserAsync(UpdateUserDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.CurrentEmail);
        if (user is null)
            return false;

        if (!string.IsNullOrWhiteSpace(dto.NewEmail))
            user.Email = dto.NewEmail;

        if (!string.IsNullOrWhiteSpace(dto.FullName))
            user.FullName = dto.FullName;

        if (!string.IsNullOrWhiteSpace(dto.NewPassword))
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

        if (!string.IsNullOrWhiteSpace(dto.NewRoleName))
        {
            var newRole = await _dbContext.Roles
                .FirstOrDefaultAsync(r => r.Name == dto.NewRoleName);
            if (newRole is null)
                return false;

            var existingRoles = await _dbContext.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .ToListAsync();

            _dbContext.UserRoles.RemoveRange(existingRoles);

            _dbContext.UserRoles.Add(new UserRole
            {
                UserId = user.Id,
                RoleId = newRole.Id
            });
        }

        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteUserAsync(string email)
    {
        var user = await _dbContext.Users
            .Include(u => u.UserRoles)
            .FirstOrDefaultAsync(u => u.Email == email);
        if (user is null)
            return false;

        _dbContext.UserRoles.RemoveRange(user.UserRoles);
        _userRepository.Delete(user);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<List<RoleResponseDto>> GetAllRolesAsync()
    {
        return await _dbContext.Roles
            .Select(r => new RoleResponseDto
            {
                Id = r.Id,
                Name = r.Name
            })
            .ToListAsync();
    }
}
