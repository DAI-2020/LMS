using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit;

namespace LMS.API.Tests;

public class SecurityTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public SecurityTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetSummary_WithoutAuth_Returns401()
    {
        var response = await _client.GetAsync("/api/Security/summary");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetSummary_WithValidToken_Returns200()
    {
        var token = await AuthHelper.GetTokenAsync(_client, "ahmedstudent@gmail.com", "password123");
        token.Should().NotBeNull();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync("/api/Security/summary");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var json = await response.Content.ReadAsStringAsync();
        json.Should().NotBeEmpty();
    }

    [Fact]
    public async Task UpdateSettings_WithoutAuth_Returns401()
    {
        var response = await _client.PostAsJsonAsync("/api/Security/update", new { });
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task UpdateSettings_WithWrongPassword_Returns400()
    {
        var token = await AuthHelper.GetTokenAsync(_client, "ahmedstudent@gmail.com", "password123");
        token.Should().NotBeNull();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.PostAsJsonAsync("/api/Security/update", new
        {
            CurrentPassword = "wrongpassword123",
            NewPassword = "NewSecurePass123!"
        });
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
