using System.ComponentModel.DataAnnotations;

namespace VendorCompliance.Web.Contracts;

public sealed record CreateVendorRequest(
    [Required, StringLength(200, MinimumLength = 2)] string Name);