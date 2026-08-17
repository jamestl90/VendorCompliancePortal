using Microsoft.EntityFrameworkCore;
using VendorCompliance.Domain.Vendors;

namespace VendorCompliance.Infrastructure.Persistence;

public sealed class VendorComplianceDbContext(
    DbContextOptions<VendorComplianceDbContext> options)
    : DbContext(options)
{
    public DbSet<Vendor> Vendors => Set<Vendor>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Vendor>(vendor =>
        {
            vendor.ToTable("vendors");
            vendor.HasKey(item => item.Id);
            vendor.Property(item => item.Id).ValueGeneratedNever();
            vendor.Property(item => item.Name).HasMaxLength(200).IsRequired();
            vendor.Ignore(item => item.Documents);
        });
    }
}
