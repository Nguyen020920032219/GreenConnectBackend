using System.ComponentModel.DataAnnotations;

namespace GreenConnectPlatform.Business.Models.Files;

public class EntityFileUploadRequest : FileUploadBaseRequest
{
    [Required] public Guid EntityId { get; set; }
}