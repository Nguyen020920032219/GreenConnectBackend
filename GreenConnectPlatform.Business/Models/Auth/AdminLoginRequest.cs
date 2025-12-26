using System.ComponentModel.DataAnnotations;

namespace GreenConnectPlatform.Business.Models.Auth;

public class AdminLoginRequest
{
    [Required] public string Email { get; set; } = null!;
    [Required] public string Password { get; set; } = null!;
}