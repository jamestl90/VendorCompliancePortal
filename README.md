# Vendor Compliance Portal

A modern .NET and Blazor business application for managing vendor onboarding,
compliance documents, review workflows, document expiry, and audit history.

## Product scope

The portal is intended for organizations that must verify supplier insurance,
licences, certifications, and contractual documents before approving vendors
for work.

Planned capabilities include:

- Vendor profile and onboarding management.
- Configurable compliance document requirements.
- Document expiry tracking.
- Submission, review, approval, rejection, and change-request workflows.
- Role-based access for suppliers, procurement, compliance, and administrators.
- Audit history and operational reporting.

## Architecture

The application is structured as a modular monolith with dependencies directed
toward the domain:

- `VendorCompliance.Domain`: business concepts and rules.
- `VendorCompliance.Application`: use cases and application orchestration.
- `VendorCompliance.Prototype`: early executable entry point for validating
  domain behaviour.

ASP.NET Core, Blazor, persistence, and infrastructure projects will be added as
the application grows.

## Build

Requires the .NET 10 SDK.

```powershell
dotnet restore VendorCompliancePortal.slnx
dotnet build VendorCompliancePortal.slnx --configuration Release --no-restore
dotnet test VendorCompliancePortal.slnx --configuration Release --no-build
```
