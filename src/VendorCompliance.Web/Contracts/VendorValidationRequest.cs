using System.ComponentModel.DataAnnotations;

namespace VendorCompliance.Web.Contracts;

public sealed record VendorValidationRequest(
    [Required, StringLength(100, MinimumLength = 2)] string Name,
    [Required, EmailAddress] string ContactEmail
);