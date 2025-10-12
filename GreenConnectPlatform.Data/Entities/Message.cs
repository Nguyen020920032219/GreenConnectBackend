using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Data.Entities;

[Index("ChatRoomId", Name = "idx_messages_chatroom")]
public class Message
{
    public Guid MessageId { get; set; }
    public string Content { get; set; } = null!;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public Guid ChatRoomId { get; set; }
    public Guid SenderId { get; set; }
    public virtual ChatRoom ChatRoom { get; set; } = null!;
    public virtual User Sender { get; set; } = null!;
}