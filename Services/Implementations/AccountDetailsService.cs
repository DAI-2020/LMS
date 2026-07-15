using LMS.API.DTOs.Account;
using LMS.API.Repositories.Interfaces;
using LMS.API.Services.Interfaces;

namespace LMS.API.Services.Implementations;

public class AccountDetailsService : IAccountDetailsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileService _fileService;
    private readonly IUserDeviceService _deviceService;
    private readonly IProfileAndSecurityService _profileService;

    public AccountDetailsService(
        IUnitOfWork unitOfWork,
        IFileService fileService,
        IUserDeviceService deviceService,
        IProfileAndSecurityService profileService)
    {
        _unitOfWork = unitOfWork;
        _fileService = fileService;
        _deviceService = deviceService;
        _profileService = profileService;
    }

    public async Task<AccountDetailsDto?> GetDetailsAsync(int userId)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user is null) return null;

        return new AccountDetailsDto
        {
            FullName = user.FullName,
            Email = user.Email,
            Gender = user.Gender.ToString(),
            DateOfBirth = user.DateOfBirth.HasValue
                ? user.DateOfBirth.Value.ToDateTime(TimeOnly.MinValue) : null,
            PhoneNumber = user.PhoneNumber,
            Address = user.Address
        };
    }

    public async Task<List<AccountDeviceDto>> GetDevicesAsync(int userId, string currentTokenHash)
    {
        var devices = await _deviceService.GetUserDevicesAsync(userId, currentTokenHash);
        return devices.Select(d => new AccountDeviceDto
        {
            SessionId = d.Id,
            DeviceName = d.DeviceName,
            LastUsedText = GetRelativeTime(d.LastUsed),
            IsCurrent = d.IsCurrentDevice
        }).ToList();
    }

    public async Task<bool> UpdateDetailsAsync(int userId, UpdateAccountDetailsRequestDto dto)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user is null) return false;

        if (dto.FullName is not null) user.FullName = dto.FullName;
        if (dto.PhoneNumber is not null) user.PhoneNumber = dto.PhoneNumber;
        if (dto.Address is not null) user.Address = dto.Address;
        if (dto.Gender is not null && Enum.TryParse<Enums.User.Gender>(dto.Gender, true, out var gender))
            user.Gender = gender;
        if (dto.DateOfBirth.HasValue)
            user.DateOfBirth = DateOnly.FromDateTime(dto.DateOfBirth.Value);

        if (!string.IsNullOrEmpty(dto.NewPassword) && !string.IsNullOrEmpty(dto.OldPassword))
        {
            if (dto.NewPassword != dto.ConfirmNewPassword)
                throw new InvalidOperationException("New password and confirmation do not match.");

            var result = await _profileService.ChangePasswordAsync(userId, dto.OldPassword, dto.NewPassword);
            if (!result) throw new InvalidOperationException("Old password is incorrect.");
        }

        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DisconnectDeviceAsync(int userId, int deviceId)
    {
        return await _deviceService.DisconnectDeviceAsync(userId, deviceId);
    }

    private static string GetRelativeTime(DateTime dateTime)
    {
        var diff = DateTime.UtcNow - dateTime;
        if (diff.TotalMinutes < 1) return "Just now";
        if (diff.TotalMinutes < 60) return $"{(int)diff.TotalMinutes}m ago";
        if (diff.TotalHours < 24) return $"{(int)diff.TotalHours}h ago";
        return $"{(int)diff.TotalDays}d ago";
    }
}
