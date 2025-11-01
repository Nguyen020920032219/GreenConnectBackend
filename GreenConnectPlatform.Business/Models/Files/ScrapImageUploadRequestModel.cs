namespace GreenConnectPlatform.Bussiness.Models.Files;

public class ScrapImageUploadRequestModel : FileUploadRequestModel
{
    public Guid PostId { get; set; }
}