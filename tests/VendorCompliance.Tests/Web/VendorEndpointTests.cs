
using System.Net;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using VendorCompliance.Web.Contracts;

namespace VendorCompliance.Tests.Web;

public sealed class VendorEndpointTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    // integration test database backed endpoint test!
    [Fact]
    public async Task PostThenGetVendor_ReturnPersistedVendor()
    {
        var request = new CreateVendorRequest($"Integration Vendor {Guid.NewGuid()}");

        using HttpResponseMessage postResponse = 
            await _client.PostAsJsonAsync("/api/vendors", request);

        Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

        VendorResponse? created = await postResponse.Content.ReadFromJsonAsync<VendorResponse>();

        Assert.NotNull(created);
        Assert.NotEqual(Guid.Empty, created.Id);
        Assert.Equal(request.Name, created.Name);
        Assert.Equal($"/api/vendors/{created.Id}", postResponse.Headers.Location?.OriginalString);

        using HttpResponseMessage getResponse = await _client.GetAsync($"/api/vendors/{created.Id}");

        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        VendorResponse? fetched = await getResponse.Content.ReadFromJsonAsync<VendorResponse>();

        Assert.NotNull(fetched);
        Assert.Equal(created, fetched);
    }

    [Fact]
    public async Task PostVendor_WithInvalidName_ReturnsValidationProblem()
    {
        var request = new CreateVendorRequest("A");

        using HttpResponseMessage response = await _client.PostAsJsonAsync("/api/vendors", request);
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        HttpValidationProblemDetails? problem = await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();

        Assert.NotNull(problem);
        Assert.Contains(nameof(CreateVendorRequest.Name), problem.Errors.Keys);
    }

    [Fact]
    public async Task GetVendor_WithUnknownId_ReturnNotFound()
    {
        using HttpResponseMessage response = await _client.GetAsync($"/api/vendors/{Guid.NewGuid}");
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}