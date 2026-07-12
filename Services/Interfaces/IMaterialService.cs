using LMS.API.DTOs.Material;

namespace LMS.API.Services.Interfaces;

public interface IMaterialService
{
    Task<IEnumerable<MaterialResponseDto>> GetAllAsync();

    Task<MaterialResponseDto?> GetByIdAsync(int id);

    Task AddAsync(CreateMaterialDto dto);

    Task UpdateAsync(int id, UpdateMaterialDto dto);

    Task DeleteAsync(int id);
}