namespace LMS.API.Models;

public class Quiz
{
    public int Id { get; set; }

    public int CourseId { get; set; }

    public int TopicId { get; set; }

    public int StudentId { get; set; }

    public double Score { get; set; }

    public DateTime TakenAt { get; set; }

    public Course Course { get; set; }

    public Topic Topic { get; set; }

    public User Student { get; set; }
}
