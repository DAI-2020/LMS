using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit;

namespace LMS.API.Tests;

public class SecurityAuditTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public SecurityAuditTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task LiveSessions_GetFiltered_WithoutAuth_Returns401()
    {
        var response = await _client.GetAsync("/api/LiveSessions");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task LiveSessions_GetById_WithoutAuth_Returns401()
    {
        var response = await _client.GetAsync("/api/LiveSessions/1");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task LiveSessions_GetUpcoming_WithoutAuth_Returns401()
    {
        var response = await _client.GetAsync("/api/LiveSessions/upcoming");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task LiveSessions_GetToday_WithoutAuth_Returns401()
    {
        var response = await _client.GetAsync("/api/LiveSessions/today");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task LiveSessions_GetCompleted_WithoutAuth_Returns401()
    {
        var response = await _client.GetAsync("/api/LiveSessions/completed");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task LiveSessions_GetRecorded_WithoutAuth_Returns401()
    {
        var response = await _client.GetAsync("/api/LiveSessions/recorded");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task LiveSessions_GetMissed_WithoutAuth_Returns401()
    {
        var response = await _client.GetAsync("/api/LiveSessions/missed");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task SessionTimeline_GetTimeline_WithoutAuth_Returns401()
    {
        var response = await _client.GetAsync("/api/SessionTimeline");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task SessionTimeline_GetPaginated_WithoutAuth_Returns401()
    {
        var response = await _client.GetAsync("/api/SessionTimeline/paginated");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task LiveSessions_Create_WithoutAuth_Returns401()
    {
        var response = await _client.PostAsJsonAsync("/api/LiveSessions", new { });
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task LiveSessions_Create_AsStudent_Returns403()
    {
        var token = await AuthHelper.GetTokenAsync(_client, "ahmedstudent@gmail.com", "password123");
        token.Should().NotBeNull();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("/api/LiveSessions", new
        {
            Title = "Test",
            Type = "Lecture",
            Mode = "Online",
            ScheduledAt = DateTime.UtcNow.AddDays(1)
        });
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task AiAssistant_Chat_WithoutAuth_Returns401()
    {
        var response = await _client.PostAsJsonAsync("/api/AiAssistant/chat", new { });
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Auth_DebugToken_WithoutAuth_Returns401()
    {
        var response = await _client.GetAsync("/api/Auth/debug-token");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Auth_DebugToken_AsStudent_Returns403()
    {
        var token = await AuthHelper.GetTokenAsync(_client, "ahmedstudent@gmail.com", "password123");
        token.Should().NotBeNull();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/Auth/debug-token");
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Login_DoesNotLeakDebugInfo()
    {
        var response = await _client.PostAsJsonAsync("/api/Auth/login", new
        {
            Email = "nonexistent@test.com",
            Password = "wrongpassword"
        });
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        var json = await response.Content.ReadAsStringAsync();
        json.Should().NotContain("debug");
        json.Should().NotContain("ErrorDetail");
        json.Should().NotContain("stack");
    }

    [Fact]
    public async Task GraduationProject_UpdateStatus_AsStudent_Returns403()
    {
        var token = await AuthHelper.GetTokenAsync(_client, "ahmedstudent@gmail.com", "password123");
        token.Should().NotBeNull();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PutAsJsonAsync("/api/GraduationProject/1/status", "Completed");
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task GraduationProject_Delete_AsStudent_Returns403()
    {
        var token = await AuthHelper.GetTokenAsync(_client, "ahmedstudent@gmail.com", "password123");
        token.Should().NotBeNull();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync("/api/GraduationProject/1");
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
