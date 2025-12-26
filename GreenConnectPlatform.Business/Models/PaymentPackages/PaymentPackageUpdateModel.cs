using System.ComponentModel.DataAnnotations;

namespace GreenConnectPlatform.Business.Models.PaymentPackages;

public class PaymentPackageUpdateModel
{
    public string? Name { get; set; }
    public string? Description { get; set; }

    [Range(0.1, double.MaxValue, ErrorMessage = "Price phải lớn hơn 0.")]
    public decimal? Price { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Amount phải lớn hơn 0.")]
    public int? ConnectionAmount { get; set; }

    [Range(0, 1, ErrorMessage = "Chỉ có 2 giá trị : 0 - Miễn phí, 1 - Trả phí")]
    public int? PackageType { get; set; }
}