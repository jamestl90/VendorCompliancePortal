var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpLogging();
var app = builder.Build();

app.UseHttpLogging();
app.MapGet("/api/status", (IConfiguration config, 
                           IHostEnvironment hostEnv, 
                           ILogger<Program> logger) =>
    {
        logger.LogInformation("Test Message");
        return Results.Ok(new
        {
            status = "ok",
            application = config["Portal:Name"],
            environment = hostEnv.EnvironmentName
        });
    }
);
app.Run();

public partial class Program
{
    
}