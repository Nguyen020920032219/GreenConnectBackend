using System.ComponentModel.DataAnnotations;

namespace GreenConnectPlatform.Business.Models.PaymentPackages;

public class PaymentPackageUpdateModel
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    [Range(0.1, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
    public decimal? Price { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
    public int? ConnectionAmount { get; set; }
    [Range(0, 1, ErrorMessage = "Value must be: 0 - Free, 1 - Paid")]
    public int? PackageType { get; set; }
}