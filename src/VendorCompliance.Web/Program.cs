using VendorCompliance.Web.Contracts;
using Microsoft.EntityFrameworkCore;
using VendorCompliance.Infrastructure.Persistence;
using VendorCompliance.Domain.Vendors;

var builder = WebApplication.CreateBuilder(args);

// add services here!
builder.Services.AddOpenApi();
builder.Services.AddHttpLogging();
builder.Services.AddValidation();
builder.Services.AddProblemDetails();

// add database connection
string connectionString = builder.Configuration.GetConnectionString("VendorCompliance") ??
                                                    throw new InvalidOperationException(
                                                    "Connection string 'VendorCompliance' is not configured.");

builder.Services.AddDbContext<VendorComplianceDbContext>(options => options.UseNpgsql(connectionString));

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

app.MapPost("/api/vendors", async Task<IResult> (
    CreateVendorRequest request,
    VendorComplianceDbContext dbContext,
    CancellationToken ct) =>
{
    var vendor = new Vendor(Guid.NewGuid(), request.Name);   

    dbContext.Vendors.Add(vendor);
    await dbContext.SaveChangesAsync(ct);

    return Results.Created(
        $"/api/vendors/{vendor.Id}",
        new VendorResponse(vendor.Id, vendor.Name));
});

app.MapGet("/api/vendors/{id:guid}", async Task<IResult> (
    Guid id,
    VendorComplianceDbContext dbContext,
    CancellationToken ct) =>
{
    Vendor? vendor = await dbContext.Vendors.AsNoTracking().SingleOrDefaultAsync(item => item.Id == id, ct);

    return vendor is null ? Results.NotFound() : Results.Ok(new VendorResponse(vendor.Id, vendor.Name));   
});

app.Run();

public partial class Program
{
    
}