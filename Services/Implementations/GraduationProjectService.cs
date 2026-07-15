using LMS.API.DTOs.GraduationProject;
using LMS.API.Enums.GraduationProjectEnums;
using LMS.API.Models;
using LMS.API.Repositories.Interfaces;
using LMS.API.Services.Interfaces;

namespace LMS.API.Services.Implementations;

public class GraduationProjectService : IGraduationProjectService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileService _fileService;

    public GraduationProjectService(IUnitOfWork unitOfWork, IFileService fileService)
    {
        _unitOfWork = unitOfWork;
        _fileService = fileService;
    }

    public async Task<GraduationProjectResponseDto?> SubmitProjectAsync(SubmitGraduationProjectDto dto)
    {
        var allowedExtensions = new[] { ".docx", ".pdf", ".jpeg", ".jpg" };
        var extension = Path.GetExtension(dto.UploadDocument.FileName).ToLowerInvariant();

        if (!allowedExtensions.Contains(extension))
            throw new InvalidOperationException("Only docx, pdf, and jpeg files are allowed.");

        if (dto.UploadDocument.Length > 50 * 1024 * 1024)
            throw new InvalidOperationException("File size must not exceed 50 MB.");

        var fileUrl = await _fileService.UploadSubmissionAsync(dto.UploadDocument);

        var project = new GraduationProjectSubmission
        {
            StudentId = dto.StudentId,
            ProjectName = dto.ProjectName,
            LeadProject = dto.LeadProject,
            DescriptionProject = dto.DescriptionProject,
            UploadDocumentProject = fileUrl,
            ProjectStatus = GraduationProjectStatus.ProjectInitiation,
            SubmittedAt = DateTime.UtcNow
        };

        await _unitOfWork.GraduationProjects.AddAsync(project);
        await _unitOfWork.SaveChangesAsync();

        return MapToResponse(project);
    }

    public async Task<GraduationProjectResponseDto?> GetByIdAsync(int id)
    {
        var project = await _unitOfWork.GraduationProjects.GetByIdAsync(id);
        return project is null ? null : MapToResponse(project);
    }

    public async Task<IEnumerable<GraduationProjectResponseDto>> GetByStudentIdAsync(int studentId)
    {
        var projects = await _unitOfWork.GraduationProjects.GetByStudentIdAsync(studentId);
        return projects.Select(MapToResponse);
    }

    public async Task<IEnumerable<GraduationProjectResponseDto>> GetAllAsync()
    {
        var projects = await _unitOfWork.GraduationProjects.GetAllAsync();
        return projects.Select(MapToResponse);
    }

    public async Task<GraduationProjectResponseDto?> UpdateStatusAsync(int id, string status)
    {
        var project = await _unitOfWork.GraduationProjects.GetByIdAsync(id);
        if (project is null) return null;

        if (!Enum.TryParse<GraduationProjectStatus>(status, true, out var newStatus))
            throw new InvalidOperationException("Invalid project status.");

        project.ProjectStatus = newStatus;
        _unitOfWork.GraduationProjects.Update(project);
        await _unitOfWork.SaveChangesAsync();

        return MapToResponse(project);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var project = await _unitOfWork.GraduationProjects.GetByIdAsync(id);
        if (project is null) return false;

        _unitOfWork.GraduationProjects.Delete(project);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<ProjectUploadMetadataDto> GetUploadMetadataAsync()
    {
        var courses = await _unitOfWork.Courses.GetAllAsync();
        var users = await _unitOfWork.Users.GetAllAsync();

        return new ProjectUploadMetadataDto
        {
            ProjectNames = courses.Select(c => new LookupDto { Id = c.Id, Name = c.Title }).ToList(),
            ProjectLeads = users.Select(u => new LookupDto { Id = u.Id, Name = u.FullName }).ToList()
        };
    }

    private static GraduationProjectResponseDto MapToResponse(GraduationProjectSubmission project)
    {
        return new GraduationProjectResponseDto
        {
            Id = project.Id,
            StudentId = project.StudentId,
            ProjectName = project.ProjectName,
            LeadProject = project.LeadProject,
            DescriptionProject = project.DescriptionProject,
            UploadDocumentProject = project.UploadDocumentProject,
            ProjectStatus = project.ProjectStatus,
            SubmittedAt = project.SubmittedAt
        };
    }
}
