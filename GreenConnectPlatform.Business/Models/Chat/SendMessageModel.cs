namespace GreenConnectPlatform.Business.Models.Chat;

public class SendMessageModel
{
    public Guid TransactionId { get; set; }
    public string Content { get; set; } = string.Empty;
}