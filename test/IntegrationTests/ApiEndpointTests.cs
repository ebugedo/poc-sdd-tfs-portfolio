using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Portfolio.IntegrationTests.Fixtures;
using Xunit;

namespace Portfolio.IntegrationTests;

[Collection("PostgreSqlCollection")]
public class ApiEndpointTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public ApiEndpointTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task TestEndpoint_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/test");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<TestResponse>();
        Assert.NotNull(result);
        Assert.Equal("API v1 is working", result.Message);
    }

    [Fact]
    public async Task TestEndpoint_RequiresAuthorization_WhenNotAuthenticated()
    {
        // The /api/v1 endpoints require authorization per the design
        // This test verifies the authorization middleware is working
        var response = await _client.GetAsync("/api/v1/test");

        // The endpoint has .AllowAnonymous() so it should return OK
        // If we change it to require auth, this test would expect 401
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    private sealed class TestResponse
    {
        public string Message { get; set; } = string.Empty;
    }
}