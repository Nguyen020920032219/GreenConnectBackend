namespace GreenConnectPlatform.Business.Models.Exceptions;

public class ApiExceptionModel : Exception
{
    public ApiExceptionModel(int statusCode, string errorCode, string message) : base(message)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
    }

    public int StatusCode { get; set; }
    public string ErrorCode { get; set; }
}