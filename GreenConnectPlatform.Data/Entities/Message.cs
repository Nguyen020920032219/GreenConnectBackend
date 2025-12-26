namespace GreenConnectPlatform.Data.Entities;

public class Message
{
    public Guid MessageId { get; set; }

    public Guid ChatRoomId { get; set; }

    public Guid SenderId { get; set; }

    public string Content { get; set; } = null!;

    public bool IsRead { get; set; } = false;

    public DateTime Timestamp { get; set; }

    public virtual ChatRoom ChatRoom { get; set; } = null!;

    public virtual User Sender { get; set; } = null!;
}