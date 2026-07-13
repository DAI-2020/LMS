using System.ComponentModel.DataAnnotations;

namespace LMS.API.DTOs.Quiz;

public class UpdateQuizDto
{
    [Range(0, 100)]
    public double? Score { get; set; }
}
