namespace GreenConnectPlatform.Data.Entities;

public class ChatParticipant
{
    public Guid UserId { get; set; }

    public Guid ChatRoomId { get; set; }

    public DateTime JoinedAt { get; set; }

    public virtual ChatRoom ChatRoom { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}