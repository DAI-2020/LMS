using System.ComponentModel.DataAnnotations;

namespace LMS.API.DTOs.Course
{
    public class UpdateCourseDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }
    }
}
