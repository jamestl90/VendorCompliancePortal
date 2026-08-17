# Purpose

To keep myself up to date with the latest in .net applications development I've come up with this project to work on from time to time. 

# Vendor Compliance Portal

A portfolio business application being built with modern .NET and Blazor for
vendor onboarding, compliance documents, review workflows, document expiry, and
audit history.

**Current milestone:** the repository contains a deterministic compliance
domain, an Application-layer use case, an executable prototype, xUnit tests, and
a GitHub Actions CI pipeline.

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

The application is a modular monolith with dependencies directed inward toward
the Domain project:

```text
VendorCompliance.Prototype  ->  VendorCompliance.Application  ->  VendorCompliance.Domain
VendorCompliance.Tests      ->  VendorCompliance.Application
VendorCompliance.Tests      ->  VendorCompliance.Domain
```

- `VendorCompliance.Domain` contains vendor, document, requirement, and
  compliance concepts. Its evaluator is synchronous, deterministic, and free of
  UI, persistence, and infrastructure dependencies.
- `VendorCompliance.Application` exposes use cases and coordinates Domain
  behavior without reimplementing business rules.
- `VendorCompliance.Prototype` is the current composition root and executable
  host used to exercise the application with deterministic demo data.
- `VendorCompliance.Tests` references Domain and Application and currently
  verifies the compliance rules with xUnit.

Expected business failures are returned as a `ComplianceAssessment` containing
`IsCompliant` and explainable failure records. Invalid method inputs are rejected
with exceptions. Compliance dates use explicit `DateOnly` values rather than the
system clock, which keeps tests and CI runs repeatable.

## Technology stack

### Implemented now

| Area | Technology and practice |
| --- | --- |
| Runtime and language | .NET 10, C#, SDK-style projects, nullable reference types, implicit global usings |
| Solution structure | XML-based `.slnx` solution, modular-monolith project boundaries, and separate Domain, Application, Infrastructure, Web, Prototype, and Tests projects |
| Domain design | Encapsulated business rules, constructor injection, LINQ, result-oriented error handling, read-only result collections, deterministic date handling |
| Web application and APIs | ASP.NET Core Minimal APIs, dependency injection, configuration, validation, Problem Details, OpenAPI, HTTP logging, and asynchronous endpoints |
| Data access | Entity Framework Core 10, Npgsql, PostgreSQL 18, `DbContext` mapping, code-first migrations, async persistence, and no-tracking queries |
| Automated testing | xUnit, Microsoft.NET.Test.Sdk, xUnit Visual Studio runner, Coverlet collector, unit tests, and ASP.NET Core integration tests using `WebApplicationFactory` and PostgreSQL |
| Containers | Docker Compose with a health-checked PostgreSQL development database |
| Continuous integration | GitHub Actions on Ubuntu with a PostgreSQL service container, .NET tool restore, EF Core migration application, Release build, and tests on pushes and pull requests to `main`, plus manual runs |
| Source control | Git feature branches and a pull-request workflow with CI checks before merge |

### Planned and not yet implemented

These technologies are part of the project roadmap but should not be read as
features already present in this repository:

| Area | Planned technology |
| --- | --- |
| Web UI | Blazor Web App and Razor components |
| Security | ASP.NET Core Identity, role- and policy-based authorization |
| UI testing | bUnit component tests and broader integration tests |
| Containers | An OCI-compatible application image and production container configuration |
| Observability | OpenTelemetry tracing and metrics |
| Delivery | Expanded GitHub Actions CI/CD, deployment artifacts and gates, optional Azure deployment |

Amazon Elastic Container Service (AWS ECS) is not currently used. A deployment
target should only be listed as implemented after its container and deployment
configuration exists in the repository.

## Continuous integration

The workflow at `.github/workflows/ci.yml` runs on pushes and pull requests to
`main`, and can also be started manually. It uses the .NET 10 SDK on
`ubuntu-latest` and performs:

1. Package restore.
2. Release build without a second restore.
3. Test execution without a second build.

## Build

Requires the .NET 10 SDK. The repository pins SDK `10.0.302` and permits
roll-forward to the latest compatible feature band.

```powershell
dotnet restore VendorCompliancePortal.slnx
dotnet build VendorCompliancePortal.slnx --configuration Release --no-restore
dotnet test VendorCompliancePortal.slnx --configuration Release --no-build
```
