using LMS.API.Enums.TasksEnums;

namespace LMS.API.DTOs.Task
{
    public class TaskResponseDto
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public int? SessionId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string TaskType { get; set; }

        public AssignmentStatus AssignmentStatus { get; set; }

        public DateTime DueDate { get; set; }

        public string AllowedExtensions { get; set; }
    }
}
