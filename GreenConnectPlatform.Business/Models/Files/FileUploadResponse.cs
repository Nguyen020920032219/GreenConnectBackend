namespace GreenConnectPlatform.Business.Models.Files;

public class FileUploadResponse
{
    public string UploadUrl { get; set; } = null!;
    public string FilePath { get; set; } = null!;
}