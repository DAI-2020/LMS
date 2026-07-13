using LMS.API.DTOs.AIAssistantChat;
using LMS.API.Models;
using LMS.API.Repositories.Interfaces;
using LMS.API.Services.Interfaces;

namespace LMS.API.Services.Implementations;

public class AiAssistantService : IAiAssistantService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAIService _aiService;

    public AiAssistantService(IUnitOfWork unitOfWork, IAIService aiService)
    {
        _unitOfWork = unitOfWork;
        _aiService = aiService;
    }

    public async Task<AiChatResponseDto?> ChatAsync(AiChatRequestDto dto)
    {
        var prompt = dto.Feature.ToLower() switch
        {
            "help me writing" => BuildHelpMeWritingPrompt(dto.Message),
            "create a study plan" => BuildStudyPlanPrompt(dto.Message),
            "summarize lesson" => BuildSummarizeLessonPrompt(dto.Message),
            _ => dto.Message
        };

        var response = await _aiService.GenerateResponse(prompt);

        var chat = new AIAssistantChat
        {
            StudentId = dto.StudentId,
            CourseId = dto.CourseId,
            UserQuery = dto.Message,
            AIResponse = response,
            AskedAt = DateTime.UtcNow
        };

        await _unitOfWork.AIChats.AddAsync(chat);
        await _unitOfWork.SaveChangesAsync();

        return new AiChatResponseDto
        {
            Id = chat.Id,
            StudentId = chat.StudentId,
            CourseId = chat.CourseId,
            UserQuery = chat.UserQuery,
            AIResponse = chat.AIResponse,
            AskedAt = chat.AskedAt
        };
    }

    public async Task<AiChatResponseDto?> HelpMeWritingAsync(int studentId, int courseId, string text)
    {
        var dto = new AiChatRequestDto
        {
            StudentId = studentId,
            CourseId = courseId,
            Message = text,
            Feature = "Help Me Writing"
        };
        return await ChatAsync(dto);
    }

    public async Task<AiChatResponseDto?> CreateStudyPlanAsync(int studentId, int courseId, string additionalInfo)
    {
        var dto = new AiChatRequestDto
        {
            StudentId = studentId,
            CourseId = courseId,
            Message = additionalInfo,
            Feature = "Create a study plan"
        };
        return await ChatAsync(dto);
    }

    public async Task<AiChatResponseDto?> SummarizeLessonAsync(int studentId, int courseId, string lessonContent)
    {
        var dto = new AiChatRequestDto
        {
            StudentId = studentId,
            CourseId = courseId,
            Message = lessonContent,
            Feature = "Summarize Lesson"
        };
        return await ChatAsync(dto);
    }

    private static string BuildHelpMeWritingPrompt(string text)
    {
        return $"You are an academic writing assistant. Help improve and refine the following text while maintaining its original meaning and intent. Provide suggestions for better clarity, grammar, and style:\n\n{text}";
    }

    private static string BuildStudyPlanPrompt(string additionalInfo)
    {
        return $"You are an educational planner. Create a personalized study plan based on the student's progress and schedule. Consider the following information:\n\n{additionalInfo}\n\nProvide a structured study plan with daily/weekly goals, recommended resources, and milestones.";
    }

    private static string BuildSummarizeLessonPrompt(string lessonContent)
    {
        return $"You are a lesson summarizer. Summarize the following lesson content in a concise and easy-to-understand format. Highlight key concepts, important definitions, and main takeaways:\n\n{lessonContent}";
    }
}
