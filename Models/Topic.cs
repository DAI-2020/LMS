namespace LMS.API.Models;

public class Topic
{
    public int Id { get; set; }

    public string Name { get; set; }

    public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
}
