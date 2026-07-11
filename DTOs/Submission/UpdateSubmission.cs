using LMS.API.Enums.TasksEnums;

namespace LMS.API.DTOs.Submission
{
    public class UpdateSubmission
    {
        public AssignmentStatus AssignmentStatus { get; set; }

        public string? AIGrade { get; set; }

        public string? AIFeedback { get; set; }
    }
}
