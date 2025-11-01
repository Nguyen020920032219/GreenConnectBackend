namespace GreenConnectPlatform.Bussiness.Models.Auth;

public class FirebaseRegisterRequestModel
{
    public string IdToken { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = "Household";
}