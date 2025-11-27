namespace GreenConnectPlatform.Business.Models.Chat;

public class ChatRoomModel
{
    public Guid ChatRoomId { get; set; }
    public Guid TransactionId { get; set; }
    public string PartnerName { get; set; } = "Unknown";
    public string PartnerAvatar { get; set; } = "";
    public string LastMessage { get; set; } = "";
    public DateTime? LastMessageTime { get; set; }
    public int UnreadCount { get; set; }
}