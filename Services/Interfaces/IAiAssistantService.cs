using LMS.API.DTOs.AIAssistantChat;

namespace LMS.API.Services.Interfaces;

public interface IAiAssistantService
{
    Task<AiChatResponseDto?> ChatAsync(AiChatRequestDto dto);

    Task<AiChatResponseDto?> HelpMeWritingAsync(int studentId, int courseId, string text);

    Task<AiChatResponseDto?> CreateStudyPlanAsync(int studentId, int courseId, string additionalInfo);

    Task<AiChatResponseDto?> SummarizeLessonAsync(int studentId, int courseId, string lessonContent);
}
