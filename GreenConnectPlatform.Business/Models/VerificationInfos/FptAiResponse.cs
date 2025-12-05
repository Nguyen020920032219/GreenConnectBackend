using System.Text.Json.Serialization;

namespace GreenConnectPlatform.Business.Models.VerificationInfos;

public class FptAiResponse
{
    [JsonPropertyName("errorCode")] public int ErrorCode { get; set; }

    [JsonPropertyName("errorMessage")] public string ErrorMessage { get; set; } = string.Empty;

    [JsonPropertyName("data")] public List<FptAiData>? Data { get; set; }
}