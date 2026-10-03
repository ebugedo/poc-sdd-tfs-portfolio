using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Portfolio.IntegrationTests.Fixtures;
using Xunit;

namespace Portfolio.IntegrationTests;

[Collection("PostgreSqlCollection")]
public class HealthCheckTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public HealthCheckTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task HealthCheck_ReturnsHealthy()
    {
        // Act
        var response = await _client.GetAsync("/health");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<HealthCheckResponse>();
        Assert.NotNull(result);
        Assert.Equal("Healthy", result.Status);
    }

    private sealed class HealthCheckResponse
    {
        public string Status { get; set; } = string.Empty;
    }
}