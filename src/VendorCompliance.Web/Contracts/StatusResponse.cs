namespace VendorCompliance.Web.Contracts;

public sealed record StatusResponse(
    string Status,
    string Application,
    string Environment);