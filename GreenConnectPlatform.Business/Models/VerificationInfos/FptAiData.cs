using System.Text.Json.Serialization;

namespace GreenConnectPlatform.Business.Models.VerificationInfos;

public class FptAiData
{
    [JsonPropertyName("id")] public string? Id { get; set; }

    [JsonPropertyName("name")] public string? Name { get; set; }

    [JsonPropertyName("dob")] public string? Dob { get; set; }

    [JsonPropertyName("address")] public string? Address { get; set; }

    [JsonPropertyName("home")] public string? Home { get; set; }
}