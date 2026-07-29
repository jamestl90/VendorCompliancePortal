using System;
using VendorCompliance.Domain.Compliance;
using VendorCompliance.Domain.Documents;
using VendorCompliance.Domain.Vendors;

namespace VendorCompliance.Application.Compliance;

public sealed class AssessVendorCompliance
{
    private readonly ComplianceEvaluator _evaluator;

    public AssessVendorCompliance(ComplianceEvaluator evaluator)
    {
        ArgumentNullException.ThrowIfNull(evaluator);
        _evaluator = evaluator;
    }

    public ComplianceAssessment Execute(Vendor vendor, 
        IReadOnlyCollection<DocumentRequirement> requirements, 
        DateOnly assessDate)
    {
        return _evaluator.Evaluate(vendor, requirements, assessDate);
    }
}