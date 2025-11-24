using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Models.PaymentPackages;

public class PaymentPackageOverallModel
{
    public Guid PackageId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public PackageType PackageType { get; set; }

}