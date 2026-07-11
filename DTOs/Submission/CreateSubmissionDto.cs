using System.ComponentModel.DataAnnotations;

namespace LMS.API.DTOs.Submission
{
    public class CreateSubmissionDto
    {
        [Required]
        public int TaskId { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public string FileUrl { get; set; }
    }
}
