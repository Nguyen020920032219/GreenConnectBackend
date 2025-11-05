namespace GreenConnectPlatform.Business.Models.Files;

public class FileUploadRequestModel
{
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
}