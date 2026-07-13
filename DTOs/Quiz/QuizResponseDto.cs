namespace LMS.API.DTOs.Quiz;

public class QuizResponseDto
{
    public int Id { get; set; }

    public int CourseId { get; set; }

    public int TopicId { get; set; }

    public int StudentId { get; set; }

    public double Score { get; set; }

    public DateTime TakenAt { get; set; }
}
