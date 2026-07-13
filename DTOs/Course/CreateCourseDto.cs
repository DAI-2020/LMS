using System.ComponentModel.DataAnnotations;

namespace LMS.API.DTOs.Course
{
    public class CreateCourseDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        [MaxLength(200)]
        public string Description { get; set; }

        [Required]
        public int InstructorId { get; set; }
    }
}
