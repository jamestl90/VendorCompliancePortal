using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using VendorCompliance.Web.Contracts;

namespace VendorCompliance.Tests.Web;

public sealed class VendorValidationEndpointTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task VendorValidation_create_valid_post()
    {
        var request = new VendorValidationRequest(
            Name: "Made Up Company",
            ContactEmail: "compliance@madeupcompany.com");

        using HttpResponseMessage response = await _client.PostAsJsonAsync(
            "/api/vendors/validate",
            request);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    } 

    [Fact]
    public async Task VendorValidation_create_invalid_post()
    {
        var request = new VendorValidationRequest(
            Name: "A",
            ContactEmail: "not an email!");
            
        using HttpResponseMessage response = await _client.PostAsJsonAsync(
            "/api/vendors/validate",
            request);

        HttpValidationProblemDetails? problem = 
            await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();

        Assert.NotNull(problem);
        Assert.Contains(nameof(VendorValidationRequest.Name), problem.Errors.Keys);
        Assert.Contains(nameof(VendorValidationRequest.ContactEmail), problem.Errors.Keys);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}