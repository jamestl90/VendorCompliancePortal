using VendorCompliance.Web.Contracts;

var builder = WebApplication.CreateBuilder(args);

// add services here!
builder.Services.AddOpenApi();
builder.Services.AddHttpLogging();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpLogging();
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
    }
);
app.Run();

public partial class Program
{
    
}