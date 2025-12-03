using System.Net.Http.Json;
using GreenConnectPlatform.Business.Models.Banks;
using Microsoft.Extensions.Caching.Memory;

namespace GreenConnectPlatform.Business.Services.Banks;

public class BankService : IBankService
{
    private readonly IMemoryCache _cache;
    private readonly HttpClient _httpClient;

    public BankService(HttpClient httpClient, IMemoryCache cache)
    {
        _httpClient = httpClient;
        _cache = cache;
    }

    public async Task<List<BankModel>> GetSupportedBanksAsync()
    {
        // 1. Kiểm tra Cache (Key: "SupportedBanks")
        // Cache trong 24 giờ vì danh sách ngân hàng rất ít khi đổi
        if (_cache.TryGetValue("SupportedBanks", out List<BankModel>? cachedBanks)) return cachedBanks!;

        // 2. Nếu không có Cache -> Gọi API VietQR
        try
        {
            var response =
                await _httpClient.GetFromJsonAsync<VietQrResponse<List<BankModel>>>("https://api.vietqr.io/v2/banks");

            if (response != null && response.Code == "00" && response.Data != null)
            {
                // 3. Lưu vào Cache
                var banks = response.Data;
                _cache.Set("SupportedBanks", banks, TimeSpan.FromHours(24));
                return banks;
            }
        }
        catch
        {
            // Log lỗi (nếu cần)
        }

        return new List<BankModel>(); // Trả về rỗng nếu lỗi
    }
}