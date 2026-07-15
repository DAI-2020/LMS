using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit;

namespace LMS.API.Tests;

public class AccountDetailsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AccountDetailsTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetDetails_WithoutAuth_Returns401()
    {
        var response = await _client.GetAsync("/api/AccountDetails");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetDetails_WithValidToken_Returns200Or404()
    {
        var token = await AuthHelper.GetTokenAsync(_client, "ahmedstudent@gmail.com", "password123");
        token.Should().NotBeNull();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync("/api/AccountDetails");
        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetEditDevices_WithoutAuth_Returns401()
    {
        var response = await _client.GetAsync("/api/AccountDetails/edit-devices");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task UpdateDetails_WithoutAuth_Returns401()
    {
        var response = await _client.PutAsJsonAsync("/api/AccountDetails/update", new { });
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task DisconnectDevice_WithoutAuth_Returns401()
    {
        var response = await _client.DeleteAsync("/api/AccountDetails/disconnect-device/1");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task DisconnectDevice_WithInvalidSessionId_Returns404()
    {
        var token = await AuthHelper.GetTokenAsync(_client, "ahmedstudent@gmail.com", "password123");
        token.Should().NotBeNull();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.DeleteAsync("/api/AccountDetails/disconnect-device/999999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
