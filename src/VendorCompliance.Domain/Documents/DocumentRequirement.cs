using System;

namespace VendorCompliance.Domain.Documents;

public sealed record class DocumentRequirement
{
    public DocumentType Type { get; }

    public DocumentRequirement(DocumentType type)
    {
        if (!Enum.IsDefined(type))
        {
            throw new ArgumentOutOfRangeException(nameof(type));
        }
        Type = type;
    }
}