using VendorCompliance.Domain.Documents;
using VendorCompliance.Domain.Vendors;

namespace VendorCompliance.Prototype.SampleData;

public sealed record DemoScenario(
    string Code,
    string Name,
    Vendor Vendor,
    IReadOnlyCollection<DocumentRequirement> Requirements,
    DateOnly AssessedOn);
