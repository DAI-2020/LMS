using LMS.API.Enums.TasksEnums;
using System.ComponentModel.DataAnnotations;

namespace LMS.API.DTOs.CourseTask
{
    public class CreateCourseTaskDto
    {
        [Required]
        public int CourseId { get; set; }

        public int? SessionId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Description { get; set; }

        [Required]
        public TaskType TaskType { get; set; }

        [Required]
        public AssignmentStatus AssignmentStatus { get; set; }

        public DateTime DueDate { get; set; }

        public string AllowedExtensions { get; set; }
    }
}
