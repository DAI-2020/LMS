namespace LMS.API.Services.Implementations;

public class AIService : IAIService
{
    public Task<string> GenerateResponse(string question)
    {
        var response = $"AI response to: {question}";
        return Task.FromResult(response);
    }

    public Task<(string grade, string feedback)> EvaluateSubmission(string content)
    {
        var grade = "85";
        var feedback = "Good submission with minor improvements needed.";
        return Task.FromResult((grade, feedback));
    }
}
