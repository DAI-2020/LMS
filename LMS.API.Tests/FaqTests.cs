using System.Net;
using FluentAssertions;
using Xunit;

namespace LMS.API.Tests;

public class FaqTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public FaqTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetFaqs_WithoutAuth_Returns200()
    {
        var response = await _client.GetAsync("/api/Faq");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetFaqs_ReturnsNonEmptyContent()
    {
        var response = await _client.GetAsync("/api/Faq");
        var json = await response.Content.ReadAsStringAsync();
        json.Should().NotBeNullOrEmpty();
        json.Should().Contain("How do I reset my password?");
    }
}
