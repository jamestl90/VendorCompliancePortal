using VendorCompliance.Domain.Documents;
using VendorCompliance.Domain.Vendors;

namespace VendorCompliance.Prototype.SampleData;

public static class DemoData
{
    private static readonly DateOnly ReferenceAssessmentDate = new(2026, 7, 25);

    // generate a single test use case 
    public static Vendor CreateWestlineVendor()
    {
        return CreateVendor(
            5,
            "Westline Electrical Pty Ltd",
            (501, DocumentType.PublicLiabilityInsurance, new DateOnly(2026, 11, 12)),
            (502, DocumentType.ElectricalContractorLicence, new DateOnly(2026, 6, 30)));
    }

    public static IReadOnlyCollection<DocumentRequirement> CreateRequirements() =>
    [
        new DocumentRequirement(DocumentType.PublicLiabilityInsurance),
        new DocumentRequirement(DocumentType.WorkersCompensationInsurance),
        new DocumentRequirement(DocumentType.ElectricalContractorLicence),
    ];

    // build all 6 (at time of writing) possible scenarios for testing 
    public static IReadOnlyCollection<DemoScenario> CreateAcceptanceScenarios()
    {
        return
        [
            new DemoScenario(
                "S01",
                "All current",
                CreateVendor(
                    1,
                    "Northshore Plumbing Pty Ltd",
                    (101, DocumentType.PublicLiabilityInsurance, new DateOnly(2026, 11, 12)),
                    (102, DocumentType.WorkersCompensationInsurance, new DateOnly(2026, 12, 31)),
                    (103, DocumentType.ElectricalContractorLicence, new DateOnly(2027, 1, 15))),
                CreateRequirements(),
                ReferenceAssessmentDate),

            new DemoScenario(
                "S02",
                "One missing",
                CreateVendor(
                    2,
                    "Redgum Maintenance Pty Ltd",
                    (201, DocumentType.PublicLiabilityInsurance, new DateOnly(2026, 11, 12)),
                    (202, DocumentType.ElectricalContractorLicence, new DateOnly(2027, 1, 15))),
                CreateRequirements(),
                ReferenceAssessmentDate),

            new DemoScenario(
                "S03",
                "One expired",
                CreateVendor(
                    3,
                    "Coastal Spark Services Pty Ltd",
                    (301, DocumentType.PublicLiabilityInsurance, new DateOnly(2026, 11, 12)),
                    (302, DocumentType.WorkersCompensationInsurance, new DateOnly(2026, 12, 31)),
                    (303, DocumentType.ElectricalContractorLicence, new DateOnly(2026, 6, 30))),
                CreateRequirements(),
                ReferenceAssessmentDate),

            new DemoScenario(
                "S04",
                "Expires today",
                CreateVendor(
                    4,
                    "Summit Fire Systems Pty Ltd",
                    (401, DocumentType.PublicLiabilityInsurance, ReferenceAssessmentDate),
                    (402, DocumentType.WorkersCompensationInsurance, new DateOnly(2026, 12, 31)),
                    (403, DocumentType.ElectricalContractorLicence, new DateOnly(2027, 1, 15))),
                CreateRequirements(),
                ReferenceAssessmentDate),

            new DemoScenario(
                "S05",
                "Multiple failures",
                CreateWestlineVendor(),
                CreateRequirements(),
                ReferenceAssessmentDate),

            new DemoScenario(
                "S06",
                "No requirements",
                CreateVendor(6, "Swan Valley Services Pty Ltd"),
                [],
                ReferenceAssessmentDate),
        ];
    }

    private static Vendor CreateVendor(
        int vendorId,
        string name,
        params (int Id, DocumentType Type, DateOnly ExpiresOn)[] documents)
    {
        var vendor = new Vendor(CreateGuid(vendorId), name);

        foreach (var document in documents)
        {
            vendor.SupplyDocument(
                new ComplianceDocument(
                    CreateGuid(document.Id),
                    document.Type,
                    document.ExpiresOn));
        }

        return vendor;
    }

    private static Guid CreateGuid(int value) =>
        Guid.Parse($"00000000-0000-0000-0000-{value:D12}");
}
