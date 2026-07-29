using System;
using VendorCompliance.Domain.Documents;

namespace VendorCompliance.Domain.Documents;

public sealed class ComplianceDocument
{
    public ComplianceDocument(Guid id, DocumentType type, DateOnly expiry)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("ID cannot be empty.", nameof(id));
        }
        if (!Enum.IsDefined(type))
        {
            throw new ArgumentOutOfRangeException(nameof(type), type, "Document type is not defined.");   
        }

        Id = id;
        Type = type;
        ExpiresOn = expiry;
    }

    public DocumentType Type { get; }

    public Guid Id { get; }

    public DateOnly ExpiresOn { get; }
}