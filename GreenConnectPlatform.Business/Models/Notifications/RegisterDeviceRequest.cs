using System.ComponentModel.DataAnnotations;

namespace GreenConnectPlatform.Business.Models.Notifications;

public class RegisterDeviceRequest
{
    [Required] public string FcmToken { get; set; } = null!;

    public string? Platform { get; set; } // "android", "ios"
}