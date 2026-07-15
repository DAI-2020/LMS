using LMS.API.DTOs.GraduationProject;

namespace LMS.API.Services.Interfaces;

public interface IGraduationProjectService
{
    Task<GraduationProjectResponseDto?> SubmitProjectAsync(SubmitGraduationProjectDto dto);

    Task<GraduationProjectResponseDto?> GetByIdAsync(int id);

    Task<IEnumerable<GraduationProjectResponseDto>> GetByStudentIdAsync(int studentId);

    Task<IEnumerable<GraduationProjectResponseDto>> GetAllAsync();

    Task<GraduationProjectResponseDto?> UpdateStatusAsync(int id, string status);

    Task<bool> DeleteAsync(int id);

    Task<ProjectUploadMetadataDto> GetUploadMetadataAsync();
}
