using LMS.API.DTOs.Submission;

namespace LMS.API.Services.Interfaces
{
    public interface IStudentTaskService
    {
        Task<SubmissionResponseDto?> SubmitHomeworkAsync(CreateSubmissionDto dto);
        Task<SubmissionResponseDto?> GradeHomeworkAsync(int submissionId, UpdateSubmission dto);
        Task<IEnumerable<SubmissionResponseDto>> GetSubmissionsByStudentAsync(int studentId);
    }
}
