using LMS.API.Enums.TasksEnums;

namespace LMS.API.DTOs.Task
{
    public class UpdateTaskDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public AssignmentStatus AssignmentStatus { get; set; }

        public DateTime DueDate { get; set; }

        public string AllowedExtensions { get; set; }
    }
}
