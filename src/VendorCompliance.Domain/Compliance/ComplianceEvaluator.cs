using System;
using VendorCompliance.Domain.Documents;
using VendorCompliance.Domain.Vendors;

namespace VendorCompliance.Domain.Compliance;

public sealed class ComplianceEvaluator
{
    public ComplianceAssessment Evaluate(Vendor vendor, 
        IReadOnlyCollection<DocumentRequirement> requirements,
        DateOnly assessedOn)
    {
        if (vendor == null)
        {
            throw new ArgumentNullException(nameof(vendor));
        }
        if (requirements == null)
        {
            throw new ArgumentNullException(nameof(requirements));
        }

        var failures = new List<ComplianceFailure>();

        foreach (var req in requirements)
        {
            // Check if this requirement type exists in vendor documents
            var vendorDoc = vendor.Documents.FirstOrDefault(x => x.Type == req.Type);

            if (vendorDoc == null) // didn't exist, add failure for missing
            {   
                failures.Add(new ComplianceFailure(ComplianceFailureReason.Missing, req.Type));
            }
            else if (vendorDoc.ExpiresOn < assessedOn) // passed expiry, add failure for expiry
            {
                failures.Add(new ComplianceFailure(ComplianceFailureReason.Expired, req.Type));
            }
        }

        // create the assessment 
        ComplianceAssessment complianceAssessment = new ComplianceAssessment(vendor.Id, assessedOn, failures);

        return complianceAssessment;
    }
}