using LMS.API.DTOs.Assignment;

namespace LMS.API.Services.Interfaces;

public interface IAssignmentService
{
    Task<List<AssignmentCardDto>> GetAssignmentsAsync(int studentId, AssignmentFilterDto filter);
    Task<AssignmentDetailsDto?> GetAssignmentDetailsAsync(int assignmentId, int studentId);
    Task<bool> SubmitAssignmentAsync(int studentId, SubmitAssignmentRequestDto dto);
}
