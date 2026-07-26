using System;
using VendorCompliance.Domain.Documents;

namespace VendorCompliance.Domain.Vendors;

public sealed class Vendor
{
    private readonly Dictionary<DocumentType, ComplianceDocument> _documents = [];

    public IReadOnlyCollection<ComplianceDocument> Documents => _documents.Values;

    public Vendor(Guid id, string name)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("GUID:id can't be empty", nameof(id));
        }
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("string:name can't be empty or null", nameof(name));
        }

        Id = id;
        Name = name;
    }

    public Guid Id { get; }

    public string Name { get; }

    public void SupplyDocument(ComplianceDocument document)
    {
        if (document == null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        if (_documents.ContainsKey(document.Type))
        {
            throw new InvalidOperationException("Document already exists");
        }

        _documents.Add(document.Type, document);
    }
}

