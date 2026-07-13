using LMS.API.Services.Interfaces;

namespace LMS.API.Services.Implementations
{
    public class AIService : IAIService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public AIService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> GenerateResponse(string question)
        {
            var apiKey = _configuration["AI:ApiKey"];
            var apiUrl = _configuration["AI:ApiUrl"] ?? "https://api.openai.com/v1/chat/completions";

            var requestBody = new
            {
                model = _configuration["AI:Model"] ?? "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "system", content = "You are a helpful educational AI assistant for a Learning Management System. Provide clear, accurate, and helpful responses to student questions." },
                    new { role = "user", content = question }
                },
                max_tokens = 1000,
                temperature = 0.7
            };

            var json = System.Text.Json.JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

            var response = await _httpClient.PostAsync(apiUrl, content);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            var doc = System.Text.Json.JsonDocument.Parse(responseJson);

            return doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString() ?? "No response generated.";
        }

        public async Task<(string grade, string feedback)> EvaluateSubmission(string content)
        {
            var prompt = $"Evaluate the following student submission. Provide a grade (A, B, C, D, F) and detailed feedback:\n\n{content}";

            var response = await GenerateResponse(prompt);

            var lines = response.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var grade = lines.FirstOrDefault(l => l.Contains("grade", StringComparison.OrdinalIgnoreCase)) ?? "N/A";
            var feedback = string.Join(" ", lines.Where(l => !l.Contains("grade", StringComparison.OrdinalIgnoreCase)));

            return (grade, feedback);
        }
    }
}
