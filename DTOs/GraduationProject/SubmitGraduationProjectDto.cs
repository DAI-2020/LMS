using System.ComponentModel.DataAnnotations;

namespace LMS.API.DTOs.GraduationProject
{
    public class SubmitGraduationProjectDto
    {
        [Required]
        public int StudentId { get; set; }

        [Required]
        [MaxLength(200)]
        public string ProjectName { get; set; }

        [Required]
        [MaxLength(150)]
        public string LeadProject { get; set; }

        [MaxLength(2000)]
        public string DescriptionProject { get; set; }

        [Required]
        public IFormFile Document { get; set; }
    }
}
