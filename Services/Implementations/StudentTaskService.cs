using LMS.API.DTOs.Submission;
using LMS.API.Enums.TasksEnums;
using LMS.API.Models;
using LMS.API.Repositories.Interfaces;
using LMS.API.Services.Interfaces;

namespace LMS.API.Services.Implementations
{
    public class StudentTaskService : IStudentTaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAIService _aiService;

        public StudentTaskService(IUnitOfWork unitOfWork, IAIService aiService)
        {
            _unitOfWork = unitOfWork;
            _aiService = aiService;
        }

        public async Task<SubmissionResponseDto?> SubmitHomeworkAsync(CreateSubmissionDto dto)
        {
            var task = await _unitOfWork.CourseTasks.GetByIdAsync(dto.TaskId);
            if (task is null) return null;

            var submission = new TaskSubmission
            {
                TaskId = dto.TaskId,
                StudentId = dto.StudentId,
                FileUrl = dto.FileUrl,
                SubmittedAt = DateTime.UtcNow,
                AssignmentStatus = AssignmentStatus.Submitted
            };

            await _unitOfWork.TaskSubmissions.AddAsync(submission);
            await _unitOfWork.SaveChangesAsync();

            return MapToResponse(submission);
        }

        public async Task<SubmissionResponseDto?> GradeHomeworkAsync(int submissionId, UpdateSubmission dto)
        {
            var submission = await _unitOfWork.TaskSubmissions.GetByIdAsync(submissionId);
            if (submission is null) return null;

            submission.AssignmentStatus = dto.AssignmentStatus;
            submission.AIGrade = dto.AIGrade;
            submission.AIFeedback = dto.AIFeedback;

            _unitOfWork.TaskSubmissions.Update(submission);
            await _unitOfWork.SaveChangesAsync();

            return MapToResponse(submission);
        }

        public async Task<IEnumerable<SubmissionResponseDto>> GetSubmissionsByStudentAsync(int studentId)
        {
            var allSubmissions = await _unitOfWork.TaskSubmissions.GetAllAsync();
            return allSubmissions
                .Where(s => s.StudentId == studentId)
                .Select(MapToResponse);
        }

        private static SubmissionResponseDto MapToResponse(TaskSubmission s)
        {
            return new SubmissionResponseDto
            {
                Id = s.Id,
                TaskId = s.TaskId,
                StudentId = s.StudentId,
                FileUrl = s.FileUrl,
                SubmittedAt = s.SubmittedAt,
                AssignmentStatus = s.AssignmentStatus,
                AIGrade = s.AIGrade,
                AIFeedback = s.AIFeedback
            };
        }
    }
}
