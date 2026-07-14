namespace LMS.API.Services.Interfaces;

public interface IAIService
{
    Task<string> GenerateResponse(string question);

    Task<(string grade, string feedback)> EvaluateSubmission(string content);
}
