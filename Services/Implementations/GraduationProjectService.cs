using LMS.API.DTOs.GraduationProject;
using LMS.API.Enums.GraduationProjectEnums;
using LMS.API.Repositories.Interfaces;
using LMS.API.Services.Interfaces;

namespace LMS.API.Services.Implementations
{
    public class GraduationProjectService : IGraduationProjectService
    {
        private readonly IGraduationProjectRepository _graduationProjectRepository;
        private readonly IUnitOfWork _unitOfWork;

        private static readonly string[] AllowedExtensions = { ".docx", ".pdf", ".jpeg", ".jpg" };
        private const long MaxFileSize = 50 * 1024 * 1024;

        public GraduationProjectService(
            IGraduationProjectRepository graduationProjectRepository,
            IUnitOfWork unitOfWork)
        {
            _graduationProjectRepository = graduationProjectRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<GraduationProjectResponseDto?> GetByStudentIdAsync(int studentId)
        {
            var project = await _graduationProjectRepository.GetByStudentIdAsync(studentId);

            if (project == null)
                return null;

            return new GraduationProjectResponseDto
            {
                Id = project.Id,
                StudentId = project.StudentId,
                StudentName = project.Student?.FullName,
                ProjectName = project.ProjectName,
                LeadProject = project.LeadProject,
                DescriptionProject = project.DescriptionProject,
                UploadDocumentPath = project.UploadDocumentPath,
                CurrentPhase = project.CurrentPhase.ToString()
            };
        }

        public async Task<GraduationProjectResponseDto> SubmitAsync(SubmitGraduationProjectDto dto)
        {
            ValidateFile(dto.Document);

            var uploadsDir = Path.Combine("uploads", "graduation-projects");
            Directory.CreateDirectory(uploadsDir);

            var fileName = $"{dto.StudentId}_{DateTime.UtcNow:yyyyMMddHHmmss}{Path.GetExtension(dto.Document.FileName)}";
            var filePath = Path.Combine(uploadsDir, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.Document.CopyToAsync(stream);
            }

            var existingProject = await _graduationProjectRepository.GetByStudentIdAsync(dto.StudentId);

            if (existingProject != null)
            {
                existingProject.ProjectName = dto.ProjectName;
                existingProject.LeadProject = dto.LeadProject;
                existingProject.DescriptionProject = dto.DescriptionProject;
                existingProject.UploadDocumentPath = filePath;
                existingProject.CurrentPhase = ProjectPhase.ProjectInitiation;

                _graduationProjectRepository.Update(existingProject);
            }
            else
            {
                var project = new Models.GraduationProjectSubmission
                {
                    StudentId = dto.StudentId,
                    ProjectName = dto.ProjectName,
                    LeadProject = dto.LeadProject,
                    DescriptionProject = dto.DescriptionProject,
                    UploadDocumentPath = filePath,
                    CurrentPhase = ProjectPhase.ProjectInitiation
                };

                await _graduationProjectRepository.AddAsync(project);
            }

            await _unitOfWork.SaveChangesAsync();

            var student = await _unitOfWork.Users.GetByIdAsync(dto.StudentId);

            return new GraduationProjectResponseDto
            {
                StudentId = dto.StudentId,
                StudentName = student?.FullName,
                ProjectName = dto.ProjectName,
                LeadProject = dto.LeadProject,
                DescriptionProject = dto.DescriptionProject,
                UploadDocumentPath = filePath,
                CurrentPhase = ProjectPhase.ProjectInitiation.ToString()
            };
        }

        private static void ValidateFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new Exception("No file uploaded.");

            if (file.Length > MaxFileSize)
                throw new Exception("File size exceeds the maximum allowed limit of 50MB.");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!AllowedExtensions.Contains(extension))
                throw new Exception("Unsupported file format. Allowed formats: docx, pdf, jpeg.");
        }
    }
}
