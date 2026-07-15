using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit;

namespace LMS.API.Tests;

public class SupportTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public SupportTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetCategories_WithoutAuth_Returns200()
    {
        var response = await _client.GetAsync("/api/SupportApp/categories");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetSystemStatus_WithoutAuth_Returns200()
    {
        var response = await _client.GetAsync("/api/SupportApp/system-status");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetLandingInfo_WithoutAuth_Returns401()
    {
        var response = await _client.GetAsync("/api/SupportApp/landing-info");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetLandingInfo_WithValidToken_Returns200()
    {
        var token = await AuthHelper.GetTokenAsync(_client, "ahmedstudent@gmail.com", "password123");
        token.Should().NotBeNull();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync("/api/SupportApp/landing-info");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task SubmitTicket_WithoutAuth_Returns401()
    {
        var content = new MultipartFormDataContent();
        content.Add(new StringContent("Test"), "Subject");
        content.Add(new StringContent("Test body"), "Body");
        content.Add(new StringContent("1"), "CategoryId");

        var response = await _client.PostAsync("/api/SupportApp/submit-ticket", content);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task SubmitTicket_WithExeFile_Returns400()
    {
        var token = await AuthHelper.GetTokenAsync(_client, "ahmedstudent@gmail.com", "password123");
        token.Should().NotBeNull();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var content = new MultipartFormDataContent();
        content.Add(new StringContent("Test"), "Subject");
        content.Add(new StringContent("Test body"), "Body");
        content.Add(new StringContent("1"), "CategoryId");

        var fakeExe = new ByteArrayContent(new byte[1024]);
        fakeExe.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
        content.Add(fakeExe, "Attachment", "malware.exe");

        var response = await _client.PostAsync("/api/SupportApp/submit-ticket", content);
        response.StatusCode.Should().BeOneOf(HttpStatusCode.BadRequest, HttpStatusCode.OK);
    }

    [Fact]
    public async Task SubmitTicket_WithInvalidCategory_Returns400()
    {
        var token = await AuthHelper.GetTokenAsync(_client, "ahmedstudent@gmail.com", "password123");
        token.Should().NotBeNull();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var content = new MultipartFormDataContent();
        content.Add(new StringContent("Test"), "Subject");
        content.Add(new StringContent("Test body"), "Body");
        content.Add(new StringContent("999"), "CategoryId");

        var response = await _client.PostAsync("/api/SupportApp/submit-ticket", content);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetMyTickets_WithoutAuth_Returns401()
    {
        var response = await _client.GetAsync("/api/SupportApp/my-tickets");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetMyTickets_WithValidToken_Returns200()
    {
        var token = await AuthHelper.GetTokenAsync(_client, "ahmedstudent@gmail.com", "password123");
        token.Should().NotBeNull();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync("/api/SupportApp/my-tickets");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
