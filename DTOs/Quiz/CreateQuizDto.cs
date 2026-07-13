using System.ComponentModel.DataAnnotations;

namespace LMS.API.DTOs.Quiz;

public class CreateQuizDto
{
    [Required]
    public int CourseId { get; set; }

    [Required]
    public int TopicId { get; set; }

    [Required]
    public int StudentId { get; set; }

    [Required]
    [Range(0, 100)]
    public double Score { get; set; }
}
