using Microsoft.EntityFrameworkCore;
using VendorCompliance.Domain.Vendors;
using VendorCompliance.Infrastructure.Persistence;

namespace VendorCompliance.Tests.Persistence; 

public sealed class VendorComplianceDbContextTests
{
    [Fact]
    public void Model_MapsVendorToVendorsTable()
    {
        var options = new DbContextOptionsBuilder<VendorComplianceDbContext>()
            .UseNpgsql("Host=localhost;Database=vendor_compliance").Options;

            using var context = new VendorComplianceDbContext(options);

            var vendor = context.Model.FindEntityType(typeof(Vendor));

            Assert.NotNull(vendor); 
            Assert.Equal("vendors", vendor.GetTableName());
            Assert.Equal(200, vendor.FindProperty(nameof(Vendor.Name))?.GetMaxLength());
    }
}