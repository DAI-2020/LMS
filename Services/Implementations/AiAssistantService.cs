using LMS.API.DTOs.AIAssistantChat;
using LMS.API.Models;
using LMS.API.Repositories.Interfaces;
using LMS.API.Services.Interfaces;

namespace LMS.API.Services.Implementations
{
    public class AiAssistantService : IAiAssistantService
    {
        private readonly IAIChatRepository _aiChatRepository;
        private readonly IAIService _aiService;
        private readonly IUnitOfWork _unitOfWork;

        public AiAssistantService(
            IAIChatRepository aiChatRepository,
            IAIService aiService,
            IUnitOfWork unitOfWork)
        {
            _aiChatRepository = aiChatRepository;
            _aiService = aiService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ChatResponseDto> AskAsync(AskQuestionDto dto)
        {
            var response = await _aiService.GenerateResponse(dto.UserQuery);

            var chat = new AIAssistantChat
            {
                StudentId = dto.StudentId,
                CourseId = dto.CourseId,
                UserQuery = dto.UserQuery,
                AIResponse = response,
                AskedAt = DateTime.UtcNow
            };

            await _aiChatRepository.AddAsync(chat);
            await _unitOfWork.SaveChangesAsync();

            return new ChatResponseDto
            {
                Id = chat.Id,
                StudentId = chat.StudentId,
                CourseId = chat.CourseId,
                UserQuery = chat.UserQuery,
                AIResponse = chat.AIResponse,
                AskedAt = chat.AskedAt
            };
        }

        public async Task<string> HelpMeWritingAsync(int studentId, int courseId, string text)
        {
            var prompt = $"Help me improve and polish the following text. Provide a refined version with better style, grammar, and clarity:\n\n{text}";

            var response = await _aiService.GenerateResponse(prompt);

            var chat = new AIAssistantChat
            {
                StudentId = studentId,
                CourseId = courseId,
                UserQuery = $"[Help Me Writing] {text}",
                AIResponse = response,
                AskedAt = DateTime.UtcNow
            };

            await _aiChatRepository.AddAsync(chat);
            await _unitOfWork.SaveChangesAsync();

            return response;
        }

        public async Task<string> CreateStudyPlanAsync(int studentId, int courseId)
        {
            var prompt = "Create a comprehensive and personalized study plan for this course. Include daily and weekly goals, recommended resources, practice exercises, and milestones. Structure it in a clear format with timelines.";

            var response = await _aiService.GenerateResponse(prompt);

            var chat = new AIAssistantChat
            {
                StudentId = studentId,
                CourseId = courseId,
                UserQuery = "[Create a study plan]",
                AIResponse = response,
                AskedAt = DateTime.UtcNow
            };

            await _aiChatRepository.AddAsync(chat);
            await _unitOfWork.SaveChangesAsync();

            return response;
        }

        public async Task<string> SummarizeLessonAsync(int studentId, int courseId, string lessonContent)
        {
            var prompt = $"Summarize the following lesson content in a concise and easy-to-review format. Highlight key concepts, definitions, and important points:\n\n{lessonContent}";

            var response = await _aiService.GenerateResponse(prompt);

            var chat = new AIAssistantChat
            {
                StudentId = studentId,
                CourseId = courseId,
                UserQuery = $"[Summarize Lesson] {lessonContent.Substring(0, Math.Min(200, lessonContent.Length))}...",
                AIResponse = response,
                AskedAt = DateTime.UtcNow
            };

            await _aiChatRepository.AddAsync(chat);
            await _unitOfWork.SaveChangesAsync();

            return response;
        }
    }
}
