namespace LMS.API.DTOs.Course
{
    public class CourseResponseDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int InstructorId { get; set; }
    }
}
