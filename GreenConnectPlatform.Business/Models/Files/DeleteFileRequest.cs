using System.ComponentModel.DataAnnotations;

namespace GreenConnectPlatform.Business.Models.Files;

public class DeleteFileRequest
{
    [Required] public string FilePath { get; set; } = null!;
}