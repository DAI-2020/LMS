namespace LMS.API.DTOs.Task
{
    public class TaskFilterDto
    {
        public string? Status { get; set; }
        public int? CourseId { get; set; }
        public int? SessionId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class TaskCardDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public string TaskType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string SubmissionStatus { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public bool IsOverdue { get; set; }
        public int? SubmissionId { get; set; }
        public string? AllowedExtensions { get; set; }
    }

    public class PaginatedTasksResponseDto
    {
        public List<TaskCardDto> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasPrevious => Page > 1;
        public bool HasNext => Page < TotalPages;
    }
}
