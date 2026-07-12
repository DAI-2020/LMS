using LMS.API.Enums.TasksEnums;

namespace LMS.API.DTOs.CourseTask
{
    public class UpdateCourseTaskDto
    {
        public string Title { get; set; }

        public string Description { get; set; }
        public TaskType TaskType { get; set; }
        public AssignmentStatus AssignmentStatus { get; set; }

        public DateTime DueDate { get; set; }

        public string AllowedExtensions { get; set; }
    }
}
