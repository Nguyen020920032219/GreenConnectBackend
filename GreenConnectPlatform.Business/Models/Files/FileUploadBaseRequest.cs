using System.ComponentModel.DataAnnotations;

namespace GreenConnectPlatform.Business.Models.Files;

public class FileUploadBaseRequest
{
    [Required] public string FileName { get; set; } = null!;

    [Required] public string ContentType { get; set; } = null!;
}