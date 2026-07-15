using System.Net;
using System.Net.Http.Headers;
using FluentAssertions;
using Xunit;

namespace LMS.API.Tests;

public class AssignmentTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AssignmentTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAssignments_WithoutAuth_Returns401()
    {
        var response = await _client.GetAsync("/api/Assignment");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task SubmitAssignment_WithExeFile_Returns400()
    {
        var token = await AuthHelper.GetTokenAsync(_client, "ahmedstudent@gmail.com", "password123");
        token.Should().NotBeNull();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var content = new MultipartFormDataContent();
        content.Add(new StringContent("1"), "AssignmentId");
        content.Add(new StringContent("PDF"), "SubmissionType");

        var fakeExe = new ByteArrayContent(new byte[1024]);
        fakeExe.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        content.Add(fakeExe, "File", "malware.exe");

        var response = await _client.PostAsync("/api/Assignment/submit", content);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("not allowed");
    }

    [Fact]
    public async Task SubmitAssignment_WithZipFile_Returns400()
    {
        var token = await AuthHelper.GetTokenAsync(_client, "ahmedstudent@gmail.com", "password123");
        token.Should().NotBeNull();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var content = new MultipartFormDataContent();
        content.Add(new StringContent("1"), "AssignmentId");
        content.Add(new StringContent("PDF"), "SubmissionType");

        var fakeZip = new ByteArrayContent(new byte[1024]);
        fakeZip.Headers.ContentType = new MediaTypeHeaderValue("application/zip");
        content.Add(fakeZip, "File", "archive.zip");

        var response = await _client.PostAsync("/api/Assignment/submit", content);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task SubmitAssignment_WithOversizedFile_Returns400()
    {
        var token = await AuthHelper.GetTokenAsync(_client, "ahmedstudent@gmail.com", "password123");
        token.Should().NotBeNull();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var content = new MultipartFormDataContent();
        content.Add(new StringContent("1"), "AssignmentId");
        content.Add(new StringContent("PDF"), "SubmissionType");

        var oversizedFile = new ByteArrayContent(new byte[11 * 1024 * 1024]);
        oversizedFile.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
        content.Add(oversizedFile, "File", "big.pdf");

        var response = await _client.PostAsync("/api/Assignment/submit", content);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("10MB");
    }

    [Fact]
    public async Task GetAssignmentDetails_WithInvalidId_Returns404()
    {
        var token = await AuthHelper.GetTokenAsync(_client, "ahmedstudent@gmail.com", "password123");
        token.Should().NotBeNull();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/Assignment/999999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task SubmitAssignment_WithPdfFile_AcceptsRequest()
    {
        var token = await AuthHelper.GetTokenAsync(_client, "ahmedstudent@gmail.com", "password123");
        token.Should().NotBeNull();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var content = new MultipartFormDataContent();
        content.Add(new StringContent("1"), "AssignmentId");
        content.Add(new StringContent("PDF"), "SubmissionType");

        var fakePdf = new ByteArrayContent(new byte[1024]);
        fakePdf.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
        content.Add(fakePdf, "File", "assignment.pdf");

        var response = await _client.PostAsync("/api/Assignment/submit", content);
        response.StatusCode.Should().NotBe(HttpStatusCode.InternalServerError);
    }
}
