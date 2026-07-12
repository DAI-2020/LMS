using LMS.API.DTOs.Material;
using LMS.API.Repositories.Interfaces;
using LMS.API.Services.Interfaces;

namespace LMS.API.Services.Implementations;

public class MaterialService : IMaterialService
{
    private readonly IUnitOfWork _unitOfWork;

    public MaterialService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<MaterialResponseDto>> GetAllAsync()
    {
        var materials = await _unitOfWork.Materials.GetAllAsync();

        return materials.Select(m => new MaterialResponseDto
        {
            Id = m.Id,
            CourseId = m.CourseId,
            SessionId = m.SessionId,
            Title = m.Title,
            MaterialType = m.MaterialType,
            AttachmentType = m.AttachmentType,
            FileUrl = m.FileUrl
        });
    }

    public async Task<MaterialResponseDto?> GetByIdAsync(int id)
    {
        var material = await _unitOfWork.Materials.GetByIdAsync(id);

        if (material == null)
            return null;

        return new MaterialResponseDto
        {
            Id = material.Id,
            CourseId = material.CourseId,
            SessionId = material.SessionId,
            Title = material.Title,
            MaterialType = material.MaterialType,
            AttachmentType = material.AttachmentType,
            FileUrl = material.FileUrl
        };
    }
    public async Task AddAsync(CreateMaterialDto dto)
    {
        var material = new Models.Material
        {
            CourseId = dto.CourseId,
            SessionId = dto.SessionId,
            Title = dto.Title,
            MaterialType = dto.MaterialType,
            AttachmentType = dto.AttachmentType,
            FileUrl = dto.FileUrl
        };

        await _unitOfWork.Materials.AddAsync(material);

        await _unitOfWork.SaveChangesAsync();
    }
    public async Task UpdateAsync(int id, UpdateMaterialDto dto)
    {
        var material = await _unitOfWork.Materials.GetByIdAsync(id);

        if (material == null)
            throw new Exception("Material not found.");

        material.Title = dto.Title;
        material.MaterialType = dto.MaterialType;
        material.AttachmentType = dto.AttachmentType;
        material.FileUrl = dto.FileUrl;

        _unitOfWork.Materials.Update(material);

        await _unitOfWork.SaveChangesAsync();
    }
    public async Task DeleteAsync(int id)
    {
        var material = await _unitOfWork.Materials.GetByIdAsync(id);

        if (material == null)
            throw new Exception("Material not found.");

        _unitOfWork.Materials.Delete(material);

        await _unitOfWork.SaveChangesAsync();
    }
}