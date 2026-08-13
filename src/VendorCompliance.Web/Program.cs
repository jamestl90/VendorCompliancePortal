using VendorCompliance.Web.Contracts;

var builder = WebApplication.CreateBuilder(args);

// add services here!
builder.Services.AddOpenApi();
builder.Services.AddHttpLogging();
builder.Services.AddValidation();
builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpLogging();
app.UseExceptionHandler();
app.UseStatusCodePages();
app.MapGet("/api/status", (IConfiguration config, 
                           IHostEnvironment hostEnv, 
                           ILogger<Program> logger) =>
{
    logger.LogInformation("Test Message");
    return Results.Ok(new StatusResponse
    (
        Status: "ok",
        Application: config["Portal:Name"] ?? throw new ArgumentNullException("Portal:Name is not configured."),
        Environment: hostEnv.EnvironmentName
    ));
});

app.MapPost("/api/vendors/validate", (VendorValidationRequest request,
                                     ILogger<Program> logger) =>
{
    // Just echoing it back for now
    logger.LogInformation(
            "Validated vendor request for {VendorName}",
            request.Name);

    return Results.Ok(request); 
});
app.Run();

public partial class Program
{
    
}