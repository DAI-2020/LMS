using LMS.API.Data;
using LMS.API.DTOs.Auth;
using LMS.API.Enums.User;
using LMS.API.Models;
using LMS.API.Repositories.Interfaces;
using LMS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS.API.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly LMSDbContext _dbContext;

        public AuthService(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ITokenService tokenService,
            IConfiguration configuration,
            LMSDbContext dbContext)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _configuration = configuration;
            _dbContext = dbContext;
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user is null)
                return null;

            if (!VerifyPasswordHash(dto.Password, user.PasswordHash))
                return null;

            var roles = await GetAllUserRolesAsync(user.Id);
            if (roles.Count == 0)
                return null;

            var expiryInMinutes = int.Parse(
                _configuration["Jwt:ExpiryInMinutes"]!);

            return new AuthResponseDto
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Roles = roles,
                Token = _tokenService.GenerateToken(user, roles),
                TokenExpiry = DateTime.UtcNow.AddMinutes(expiryInMinutes)
            };
        }

        public async Task<AuthResponseDto?> RegisterAsync(RegisrerDto dto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingUser is not null)
                return null;

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = HashPassword(dto.Password),
                CreatedAt = DateTime.UtcNow,
                Gender = Gender.Male
            };

            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var studentRole = await _dbContext.Roles
                .FirstOrDefaultAsync(r => r.Name == RoleType.Student.ToString());

            if (studentRole is not null)
            {
                _dbContext.UserRoles.Add(new UserRole
                {
                    UserId = user.Id,
                    RoleId = studentRole.Id
                });
                await _unitOfWork.SaveChangesAsync();
            }

            var roles = new List<string> { RoleType.Student.ToString() };
            var expiryInMinutes = int.Parse(
                _configuration["Jwt:ExpiryInMinutes"]!);

            return new AuthResponseDto
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Roles = roles,
                Token = _tokenService.GenerateToken(user, roles),
                TokenExpiry = DateTime.UtcNow.AddMinutes(expiryInMinutes)
            };
        }

        private async Task<List<string>> GetAllUserRolesAsync(int userId)
        {
            return await _dbContext.UserRoles
                .Include(ur => ur.Role)
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.Role.Name)
                .ToListAsync();
        }

        private static bool VerifyPasswordHash(string password, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }

        private static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
