namespace GreenConnectPlatform.Business.Models.Files;

public class ScrapImageUploadRequestModel : FileUploadRequestModel
{
    public Guid PostId { get; set; }
}