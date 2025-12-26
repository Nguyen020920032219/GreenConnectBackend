using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace GreenConnectPlatform.Business.Services.Payment;

public class VnPayService : IVnPayService
{
    private readonly IConfiguration _config;

    public VnPayService(IConfiguration config)
    {
        _config = config;
    }

    public string CreatePaymentUrl(HttpContext context, string txnRef, double amount, string orderInfo)
    {
        var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
        var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
        var pay = new VnPayLibrary();

        pay.AddRequestData("vnp_Version", _config["VnPay:Version"] ?? "2.1.0");
        pay.AddRequestData("vnp_Command", "pay");
        pay.AddRequestData("vnp_TmnCode", _config["VnPay:TmnCode"]!);
        pay.AddRequestData("vnp_Amount", ((long)(amount * 100)).ToString()); // Nhân 100 theo quy tắc VNPay
        pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
        pay.AddRequestData("vnp_CurrCode", "VND");
        pay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
        pay.AddRequestData("vnp_Locale", "vn");
        pay.AddRequestData("vnp_OrderInfo", orderInfo);
        pay.AddRequestData("vnp_OrderType", "other");
        pay.AddRequestData("vnp_ReturnUrl", _config["VnPay:ReturnUrl"]!);
        pay.AddRequestData("vnp_TxnRef", txnRef);

        return pay.CreateRequestUrl(_config["VnPay:BaseUrl"]!, _config["VnPay:HashSecret"]!);
    }

    public VnPayResponseModel PaymentExecute(IQueryCollection collections)
    {
        var pay = new VnPayLibrary();
        foreach (var (key, value) in collections)
            if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                pay.AddResponseData(key, value.ToString());

        var vnp_SecureHash = collections.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
        var checkSignature = pay.ValidateSignature(vnp_SecureHash, _config["VnPay:HashSecret"]!);

        if (!checkSignature) return new VnPayResponseModel { Success = false };

        return new VnPayResponseModel
        {
            Success = true,
            OrderId = pay.GetResponseData("vnp_TxnRef"),
            PaymentId = pay.GetResponseData("vnp_TransactionNo"),
            VnPayResponseCode = pay.GetResponseData("vnp_ResponseCode"),
            OrderInfo = pay.GetResponseData("vnp_OrderInfo"),
            BankCode = pay.GetResponseData("vnp_BankCode")
        };
    }
}

// --- Helper Classes ---
public class VnPayLibrary
{
    private readonly SortedList<string, string> _requestData = new(new VnPayCompare());
    private readonly SortedList<string, string> _responseData = new(new VnPayCompare());

    public void AddRequestData(string key, string value)
    {
        if (!string.IsNullOrEmpty(value)) _requestData.Add(key, value);
    }

    public void AddResponseData(string key, string value)
    {
        if (!string.IsNullOrEmpty(value)) _responseData.Add(key, value);
    }

    public string GetResponseData(string key)
    {
        return _responseData.TryGetValue(key, out var retValue) ? retValue : string.Empty;
    }

    public string CreateRequestUrl(string baseUrl, string vnp_HashSecret)
    {
        var data = new StringBuilder();
        foreach (var kv in _requestData)
        {
            if (data.Length > 0) data.Append('&');
            data.Append(kv.Key + "=" + WebUtility.UrlEncode(kv.Value));
        }

        var queryString = data.ToString();
        var baseUrlWithQuery = baseUrl + "?" + queryString;
        var signData = queryString;
        if (signData.Length > 0)
        {
            var vnp_SecureHash = Utils.HmacSHA512(vnp_HashSecret, signData);
            baseUrlWithQuery += "&vnp_SecureHash=" + vnp_SecureHash;
        }

        return baseUrlWithQuery;
    }

    public bool ValidateSignature(string inputHash, string secretKey)
    {
        var rspRaw = GetResponseData();
        var myChecksum = Utils.HmacSHA512(secretKey, rspRaw);
        return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
    }

    private string GetResponseData()
    {
        var data = new StringBuilder();
        if (_responseData.ContainsKey("vnp_SecureHashType")) _responseData.Remove("vnp_SecureHashType");
        if (_responseData.ContainsKey("vnp_SecureHash")) _responseData.Remove("vnp_SecureHash");
        foreach (var kv in _responseData)
        {
            if (data.Length > 0) data.Append('&');
            data.Append(kv.Key + "=" + WebUtility.UrlEncode(kv.Value));
        }

        return data.ToString();
    }
}

public class VnPayCompare : IComparer<string>
{
    public int Compare(string x, string y)
    {
        if (x == y) return 0;
        if (x == null) return -1;
        if (y == null) return 1;
        var vnpCompare = CompareInfo.GetCompareInfo("en-US");
        return vnpCompare.Compare(x, y, CompareOptions.Ordinal);
    }
}

public static class Utils
{
    public static string HmacSHA512(string key, string inputData)
    {
        var hash = new StringBuilder();
        var keyBytes = Encoding.UTF8.GetBytes(key);
        var inputBytes = Encoding.UTF8.GetBytes(inputData);
        using (var hmac = new HMACSHA512(keyBytes))
        {
            var hashValue = hmac.ComputeHash(inputBytes);
            foreach (var theByte in hashValue) hash.Append(theByte.ToString("x2"));
        }

        return hash.ToString();
    }

    public static string GetIpAddress(HttpContext context)
    {
        try
        {
            var ip = context.Connection.RemoteIpAddress;
            if (ip != null && ip.AddressFamily == AddressFamily.InterNetworkV6)
                ip = Dns.GetHostEntry(ip).AddressList
                    .FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);
            return ip?.ToString() ?? "127.0.0.1";
        }
        catch
        {
            return "127.0.0.1";
        }
    }
}