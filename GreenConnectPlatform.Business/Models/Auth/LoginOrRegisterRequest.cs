using System.ComponentModel.DataAnnotations;

namespace GreenConnectPlatform.Business.Models.Auth;

public class LoginOrRegisterRequest
{
    [Required]
    public string FirebaseToken { get; set; } = null!;
}