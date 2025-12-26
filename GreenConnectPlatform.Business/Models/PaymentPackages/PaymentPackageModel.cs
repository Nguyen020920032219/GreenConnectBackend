using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Models.PaymentPackages;

public class PaymentPackageModel
{
    public Guid PackageId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int? ConnectionAmount { get; set; }
    public bool IsActive { get; set; } = true;
    public PackageType PackageType { get; set; }
}