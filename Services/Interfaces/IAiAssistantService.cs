using LMS.API.DTOs.AIAssistantChat;

namespace LMS.API.Services.Interfaces
{
    public interface IAiAssistantService
    {
        Task<ChatResponseDto> AskAsync(AskQuestionDto dto);

        Task<string> HelpMeWritingAsync(int studentId, int courseId, string text);

        Task<string> CreateStudyPlanAsync(int studentId, int courseId);

        Task<string> SummarizeLessonAsync(int studentId, int courseId, string lessonContent);
    }
}
