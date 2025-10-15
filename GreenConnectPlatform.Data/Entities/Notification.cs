namespace GreenConnectPlatform.Data.Entities;

public class Notification
{
    public Guid NotificationId { get; set; }

    public Guid RecipientId { get; set; }

    public string Content { get; set; } = null!;

    public bool IsRead { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? EntityType { get; set; }

    public Guid? EntityId { get; set; }

    public virtual User Recipient { get; set; } = null!;
}