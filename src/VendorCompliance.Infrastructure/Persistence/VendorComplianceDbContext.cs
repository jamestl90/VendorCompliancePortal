using Microsoft.EntityFrameworkCore;

namespace VendorCompliance.Infrastructure.Persistence;

public sealed class VendorComplianceDbContext(
    DbContextOptions<VendorComplianceDbContext> options)
    : DbContext(options)
{
        
}
