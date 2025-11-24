using System.ComponentModel.DataAnnotations;

namespace GreenConnectPlatform.Business.Models.PaymentPackages;

public class PaymentPackageCreateModel
{
    [Required(ErrorMessage = "Package name là bắt buộc.")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Package description là bắt buộc.")]
    public string Description { get; set; }
    [Required(ErrorMessage = "Package price là bắt buộc.")]
    [Range(0.1, double.MaxValue, ErrorMessage = "Price phải lớn hơn 0.")]
    public decimal Price { get; set; }
    [Required(ErrorMessage = "Package connection amount là bắt buộc.")]
    [Range(1, int.MaxValue, ErrorMessage = "Amount phải lớn hơn 0.")]
    public int ConnectionAmount { get; set; }
    [Required(ErrorMessage = "Package là bắt buộc.")]
    [Range(0, 1, ErrorMessage = "Chỉ có 2 giá trị : 0 - Miễn phí, 1 - Trả phí")]
    public int PackageType { get; set; }
}