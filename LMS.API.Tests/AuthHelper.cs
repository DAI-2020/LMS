using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace LMS.API.Tests;

public static class AuthHelper
{
    public static async Task<string?> GetTokenAsync(HttpClient client, string email, string password)
    {
        var response = await client.PostAsJsonAsync("/api/Auth/login", new
        {
            Email = email,
            Password = password
        });

        if (response.StatusCode != HttpStatusCode.OK)
            return null;

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        var token = doc.RootElement.GetProperty("token").GetString();
        return token;
    }
}
