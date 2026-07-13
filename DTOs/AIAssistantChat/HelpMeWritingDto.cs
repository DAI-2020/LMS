using System.ComponentModel.DataAnnotations;

namespace LMS.API.DTOs.AIAssistantChat;

public class HelpMeWritingDto
{
    [Required]
    public int CourseId { get; set; }

    [Required]
    [MaxLength(5000)]
    public string Text { get; set; } = string.Empty;
}

public class StudyPlanDto
{
    [Required]
    public int CourseId { get; set; }

    [Required]
    [MaxLength(5000)]
    public string AdditionalInfo { get; set; } = string.Empty;
}

public class SummarizeLessonDto
{
    [Required]
    public int CourseId { get; set; }

    [Required]
    [MaxLength(10000)]
    public string LessonContent { get; set; } = string.Empty;
}
