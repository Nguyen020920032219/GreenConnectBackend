using System.Globalization;
using System.Net.Http.Headers;
using System.Text.Json;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.VerificationInfos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace GreenConnectPlatform.Business.Services.AI;

public class FptAiService : IEkycService
{
    private readonly string _apiKey;
    private readonly string _baseUrl;
    private readonly HttpClient _httpClient;

    public FptAiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["FptAi:ApiKey"] ?? throw new Exception("Chưa cấu hình FptAi ApiKey");
        _baseUrl = configuration["FptAi:BaseUrl"] ?? "https://api.fpt.ai/vision/idr/vnm";
    }

    public async Task<IdCardOcrResult> ExtractIdCardInfoAsync(IFormFile imageFile)
    {
        if (imageFile == null || imageFile.Length == 0)
            return new IdCardOcrResult { IsValid = false, ErrorMessage = "File ảnh không hợp lệ." };

        using var request = new HttpRequestMessage(HttpMethod.Post, _baseUrl);
        request.Headers.Add("api_key", _apiKey);

        using var content = new MultipartFormDataContent();

        // Convert IFormFile sang Stream để gửi
        using var ms = new MemoryStream();
        await imageFile.CopyToAsync(ms);
        ms.Position = 0;

        var fileContent = new StreamContent(ms);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
        content.Add(fileContent, "image", imageFile.FileName);

        request.Content = content;

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
            throw new ApiExceptionModel(StatusCodes.Status502BadGateway, "AI_ERROR", "Lỗi kết nối đến FPT.AI");

        var jsonString = await response.Content.ReadAsStringAsync();

        // Parse JSON
        var fptResult = JsonSerializer.Deserialize<FptAiResponse>(jsonString);

        if (fptResult == null || fptResult.ErrorCode != 0 || fptResult.Data == null || !fptResult.Data.Any())
        {
            var msg = string.IsNullOrEmpty(fptResult?.ErrorMessage)
                ? "Không đọc được thông tin trên thẻ."
                : fptResult.ErrorMessage;
            return new IdCardOcrResult { IsValid = false, ErrorMessage = msg };
        }

        var data = fptResult.Data[0];

        // Validate dữ liệu quan trọng
        if (string.IsNullOrEmpty(data.Id) || string.IsNullOrEmpty(data.Name))
            return new IdCardOcrResult { IsValid = false, ErrorMessage = "Ảnh mờ hoặc không tìm thấy số CCCD/Họ tên." };

        // Parse Ngày sinh
        DateTime? dob = null;
        if (DateTime.TryParseExact(data.Dob, "dd/MM/yyyy", null, DateTimeStyles.None, out var d)) dob = d;

        return new IdCardOcrResult
        {
            IsValid = true,
            IdNumber = data.Id,
            FullName = data.Name,
            Address = data.Address ?? data.Home ?? "Việt Nam",
            Dob = dob
        };
    }
}