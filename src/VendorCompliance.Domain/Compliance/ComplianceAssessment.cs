using System;
using VendorCompliance.Domain.Documents;
using VendorCompliance.Domain.Vendors;

namespace VendorCompliance.Domain.Compliance;

public sealed class ComplianceAssessment
{
    public Guid VendorId { get; }

    public DateOnly AssessedOn { get; } 

    public bool IsCompliant => Failures.Count == 0;

    public IReadOnlyCollection<ComplianceFailure> Failures { get; }

    public ComplianceAssessment(Guid vendorId, 
        DateOnly assessedOn, 
        IReadOnlyCollection<ComplianceFailure> failures)
    {
        if (vendorId == Guid.Empty)
        {
            throw new ArgumentException("Vendor ID cannot be empty.", nameof(vendorId));
        }

        VendorId = vendorId;
        AssessedOn = assessedOn;

        ArgumentNullException.ThrowIfNull(failures);
        Failures = failures.ToList().AsReadOnly();
    }
}