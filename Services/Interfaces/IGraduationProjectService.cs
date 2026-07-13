using LMS.API.DTOs.GraduationProject;

namespace LMS.API.Services.Interfaces
{
    public interface IGraduationProjectService
    {
        Task<GraduationProjectResponseDto?> GetByStudentIdAsync(int studentId);

        Task<GraduationProjectResponseDto> SubmitAsync(SubmitGraduationProjectDto dto);
    }
}
