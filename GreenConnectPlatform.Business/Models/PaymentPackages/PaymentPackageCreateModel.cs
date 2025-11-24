using System.ComponentModel.DataAnnotations;

namespace GreenConnectPlatform.Business.Models.PaymentPackages;

public class PaymentPackageCreateModel
{
    [Required(ErrorMessage = "Package name is required.")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Package description is required.")]
    public string Description { get; set; }
    [Required(ErrorMessage = "Package price is required.")]
    [Range(0.1, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
    public decimal Price { get; set; }
    [Required(ErrorMessage = "Package connection amount is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
    public int ConnectionAmount { get; set; }
    [Required(ErrorMessage = "Package name is required.")]
    [Range(0, 1, ErrorMessage = "Value must be: 0 - Free, 1 - Paid")]
    public int PackageType { get; set; }
}