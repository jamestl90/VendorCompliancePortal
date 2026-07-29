using System;
using VendorCompliance.Domain.Compliance;
using VendorCompliance.Domain.Documents;

namespace VendorCompliance.Domain.Compliance;

public sealed record class ComplianceFailure
{
    public ComplianceFailure(ComplianceFailureReason reason, DocumentType type)
    {
        Reason = reason;
        Type = type;    
    }

    public ComplianceFailureReason Reason { get; }

    public DocumentType Type { get; }
}