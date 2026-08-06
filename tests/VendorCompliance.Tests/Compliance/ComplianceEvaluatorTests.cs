using VendorCompliance.Domain.Compliance;
using VendorCompliance.Domain.Documents;
using VendorCompliance.Domain.Vendors;

namespace VendorCompliance.Tests.Compliance;

public class ComplianceEvaluatorTests
{
    // helper to generate some document requirements 
    public static IReadOnlyCollection<DocumentRequirement> CreateRequirements() =>
    [
        new DocumentRequirement(DocumentType.PublicLiabilityInsurance),
        new DocumentRequirement(DocumentType.WorkersCompensationInsurance),
        new DocumentRequirement(DocumentType.ElectricalContractorLicence),
    ];

    [Fact]
    public void Evaluate_returns_compliant_when_every_document_is_current()
    {
        // setup
        ComplianceEvaluator evaluator = new ComplianceEvaluator();
        Vendor vendor = new Vendor(Guid.NewGuid(), "TestVendor1");
        vendor.SupplyDocument(new ComplianceDocument(
                    Guid.NewGuid(),
                    DocumentType.PublicLiabilityInsurance,
                    new DateOnly(2026, 8, 22)
                    ));
        vendor.SupplyDocument(new ComplianceDocument(
                    Guid.NewGuid(),
                    DocumentType.ElectricalContractorLicence,
                    new DateOnly(2026, 8, 22)
                    ));
        vendor.SupplyDocument(new ComplianceDocument(
                    Guid.NewGuid(),
                    DocumentType.WorkersCompensationInsurance,
                    new DateOnly(2026, 8, 22)
                    ));
        
        IReadOnlyCollection<DocumentRequirement> requirements = CreateRequirements();
        DateOnly assessedOn = new DateOnly(2026, 8, 22);

        // act
        var result = evaluator.Evaluate(vendor, requirements, assessedOn);

        // assert
        Assert.True(result.IsCompliant);
        Assert.Empty(result.Failures);
    }

    [Fact]
    public void Evaluate_returns_missing_failure_when_required_document_is_missing()
    {
        // setup
        ComplianceEvaluator evaluator = new ComplianceEvaluator();
        Vendor vendor = new Vendor(Guid.NewGuid(), "TestVendor1");
        vendor.SupplyDocument(new ComplianceDocument(
                    Guid.NewGuid(),
                    DocumentType.PublicLiabilityInsurance,
                    new DateOnly(2026, 8, 22)
                    ));
        vendor.SupplyDocument(new ComplianceDocument(
                    Guid.NewGuid(),
                    DocumentType.ElectricalContractorLicence,
                    new DateOnly(2026, 8, 22)
                    ));
        IReadOnlyCollection<DocumentRequirement> requirements = CreateRequirements();
        DateOnly assessedOn = new DateOnly(2026, 8, 22);
        
        // act
        var result = evaluator.Evaluate(vendor, requirements, assessedOn);

        // assert
        Assert.False(result.IsCompliant);
        Assert.Single(result.Failures);
        Assert.Equal(ComplianceFailureReason.Missing, result.Failures.ElementAt(0).Reason);
        Assert.Equal(DocumentType.WorkersCompensationInsurance, result.Failures.ElementAt(0).Type);
    }

    [Fact]
    public void Evaluate_returns_expired_failure_when_required_document_expired_before_assessment_date()
    {
        // setup
        ComplianceEvaluator evaluator = new ComplianceEvaluator();
        Vendor vendor = new Vendor(Guid.NewGuid(), "TestVendor1");

        vendor.SupplyDocument(new ComplianceDocument(
                    Guid.NewGuid(),
                    DocumentType.PublicLiabilityInsurance,
                    new DateOnly(2026, 8, 19)
                    ));
        vendor.SupplyDocument(new ComplianceDocument(
                    Guid.NewGuid(),
                    DocumentType.ElectricalContractorLicence,
                    new DateOnly(2026, 8, 22)
                    ));

        IReadOnlyCollection<DocumentRequirement> requirements = CreateRequirements();
        DateOnly assessedOn = new DateOnly(2026, 8, 20);

        // act
        var result = evaluator.Evaluate(vendor, requirements, assessedOn);

        // assert
        Assert.False(result.IsCompliant);
        Assert.Single(result.Failures);
        Assert.Equal(ComplianceFailureReason.Expired, result.Failures.ElementAt(0).Reason);
        Assert.Equal(DocumentType.ElectricalContractorLicence, result.Failures.ElementAt(0).Type);
    }

    [Fact]
    public void Evaluate_returns_compliant_when_required_document_expires_on_assessment_date()
    {
        // setup
        ComplianceEvaluator evaluator = new ComplianceEvaluator();
        Vendor vendor = new Vendor(Guid.NewGuid(), "TestVendor1");
    }

    [Fact]
    public void Evaluate_returns_compliant_when_no_requirements_are_supplied()
    {
        // setup
        ComplianceEvaluator evaluator = new ComplianceEvaluator();
        Vendor vendor = new Vendor(Guid.NewGuid(), "TestVendor1");
    }

    [Fact]
    public void Evaluate_returns_all_failures_when_multiple_requirements_fail()
    {
        // setup
        ComplianceEvaluator evaluator = new ComplianceEvaluator();
        Vendor vendor = new Vendor(Guid.NewGuid(), "TestVendor1");
    }
}