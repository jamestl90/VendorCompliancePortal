using VendorCompliance.Domain.Documents;

namespace VendorCompliance.Tests.Documents;

public class DocumentRequirementTests
{
    [Fact]
    public void Construct_stores_required_document_type()
    {
        DocumentRequirement requirement = new DocumentRequirement(DocumentType.PublicLiabilityInsurance);

        Assert.Equal(DocumentType.PublicLiabilityInsurance, requirement.Type);
    }
}
