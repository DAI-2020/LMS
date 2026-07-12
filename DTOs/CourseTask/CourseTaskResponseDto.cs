using LMS.API.Enums.TasksEnums;

namespace LMS.API.DTOs.CourseTask
{
    public class CourseTaskResponseDto
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public int? SessionId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public TaskType TaskType { get; set; }

        public AssignmentStatus AssignmentStatus { get; set; }

        public DateTime DueDate { get; set; }

        public string AllowedExtensions { get; set; }
    }
}
