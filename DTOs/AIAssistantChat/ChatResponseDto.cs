namespace LMS.API.DTOs.AIAssistantChat
{
    public class ChatResponseDto
    {
        public int Id { get; set; }

        public int StudentId { get; set; }

        public int CourseId { get; set; }

        public string UserQuery { get; set; }

        public string AIResponse { get; set; }

        public DateTime AskedAt { get; set; }
    }
}
