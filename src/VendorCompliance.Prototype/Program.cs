using VendorCompliance.Application.Compliance;
using VendorCompliance.Domain.Compliance;
using VendorCompliance.Prototype.SampleData;

var demoVendor = DemoData.CreateWestlineVendor();
var demoReq = DemoData.CreateRequirements();
var assessmentDate = new DateOnly(2026, 7, 25);

var evaluator = new ComplianceEvaluator();
var useCase = new AssessVendorCompliance(evaluator);

ComplianceAssessment assessmentResult = useCase.Execute(demoVendor, demoReq, assessmentDate);
Console.WriteLine("Vendor: {0}\nAssessment Date: {1:d MMMM yyyy}\nCompliant: {2}", demoVendor.Name,
assessmentResult.AssessedOn, assessmentResult.IsCompliant ? "Yes" : "No");

int i = 1;
foreach (var failure in assessmentResult.Failures)
{
    var type = string.Empty;
    switch (failure.Type)
    {
        case VendorCompliance.Domain.Documents.DocumentType.ElectricalContractorLicence:
            type = "Electrical Contractor Licence";
            break;
        case VendorCompliance.Domain.Documents.DocumentType.PublicLiabilityInsurance:
            type = "Public Liability";
            break;
        case VendorCompliance.Domain.Documents.DocumentType.WorkersCompensationInsurance:
            type = "Workers Compensation";
            break;
        default:
            type = "No reason specified";
            break;
    }
    Console.WriteLine("Failure {0}: Type: {1} - Reason: {2}", i, type, failure.Reason);
    i++;
}
