namespace GreenConnectPlatform.Data.Entities;

public class ChatRoom
{
    public Guid ChatRoomId { get; set; }

    public Guid TransactionId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<ChatParticipant> ChatParticipants { get; set; } = new List<ChatParticipant>();

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual Transaction Transaction { get; set; } = null!;
}