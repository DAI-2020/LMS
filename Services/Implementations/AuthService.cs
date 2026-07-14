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

        public async Task<LoginResult> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user is null)
                return new LoginResult
                {
                    ErrorDetail = "Debug: User not found in database for this email."
                };

            if (!VerifyPasswordHash(dto.Password, user.PasswordHash))
                return new LoginResult
                {
                    ErrorDetail = "Debug: User found, but BCrypt.Verify returned false. Check hash match."
                };

            var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();
            if (roles.Count == 0)
                return new LoginResult
                {
                    ErrorDetail = "Debug: User found and password valid, but no roles assigned in UserRoles table."
                };

            var expiryInMinutes = int.Parse(
                _configuration["Jwt:ExpiryInMinutes"]!);

            return new LoginResult
            {
                Response = new AuthResponseDto
                {
                    UserId = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Roles = roles,
                    Token = _tokenService.GenerateToken(user, roles),
                    TokenExpiry = DateTime.UtcNow.AddMinutes(expiryInMinutes)
                }
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

        private static bool VerifyPasswordHash(string password, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }

        private static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public async Task<ForgotPasswordResponseDto> ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user is null)
                return new ForgotPasswordResponseDto
                {
                    Success = true,
                    Message = "If an account with that email exists, a password reset token has been generated."
                };

            var resetToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            var expiry = DateTime.UtcNow.AddHours(1);

            return new ForgotPasswordResponseDto
            {
                Success = true,
                Message = "Password reset token generated successfully.",
                ResetToken = resetToken
            };
        }
    }
}
