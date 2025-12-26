public class VnPayResponseModel
{
    public bool Success { get; set; }
    public string OrderId { get; set; } = string.Empty; // TransactionRef
    public string PaymentId { get; set; } = string.Empty; // VNP Transaction No
    public string VnPayResponseCode { get; set; } = string.Empty;
    public string OrderInfo { get; set; } = string.Empty;
    public string BankCode { get; set; } = string.Empty;
}