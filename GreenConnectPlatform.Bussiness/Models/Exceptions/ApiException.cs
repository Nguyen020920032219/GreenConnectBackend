namespace GreenConnectPlatform.Bussiness.Models.Exceptions;

public class ApiException : Exception
{
    public ApiException(int statusCode, string errorCode, string message) : base(message)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
    }

    public int StatusCode { get; set; }
    public string ErrorCode { get; set; }
}