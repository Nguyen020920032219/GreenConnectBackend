namespace GreenConnectPlatform.Bussiness.Models.Files;

public class CheckinUploadRequestModelModel : FileUploadRequestModel
{
    public Guid TransactionId { get; set; }
}