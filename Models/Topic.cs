namespace LMS.API.Models
{
    public class Topic
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }

        public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
    }
}
