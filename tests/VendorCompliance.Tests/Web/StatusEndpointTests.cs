using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace VendorCompliance.Tests.Web;

public sealed class StatusEndpointTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetStatus_ReturnsConfiguredApplicationDetails()
    {
        using HttpResponseMessage response = await _client.GetAsync("/api/status");
        response.EnsureSuccessStatusCode();

        StatusResponse? body = await response.Content.ReadFromJsonAsync<StatusResponse>();

        Assert.NotNull(body);
        Assert.Equal("ok", body.Status);
        Assert.Equal("Vendor Compliance Portal", body.Application);
        Assert.Equal("Development", body.Environment);
    }

    private sealed record StatusResponse(
        string Status, 
        string Application,
        string Environment);
}