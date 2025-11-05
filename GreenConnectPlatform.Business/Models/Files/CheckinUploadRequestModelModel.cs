namespace GreenConnectPlatform.Business.Models.Files;

public class CheckinUploadRequestModelModel : FileUploadRequestModel
{
    public Guid TransactionId { get; set; }
}