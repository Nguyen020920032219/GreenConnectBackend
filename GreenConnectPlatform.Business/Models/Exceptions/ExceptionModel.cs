namespace GreenConnectPlatform.Business.Models.Exceptions;

public class ExceptionModel
{
    public int StatusCode { get; set; }
    public string ErrorCode { get; set; } = string.Empty;
    public string? Message { get; set; }
}