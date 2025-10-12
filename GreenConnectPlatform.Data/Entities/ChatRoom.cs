using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Data.Entities;

public class ChatRoom
{
    public Guid ChatRoomId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid TransactionId { get; set; }
    public virtual Transaction Transaction { get; set; } = null!;
    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
    public virtual ICollection<ChatParticipant> Participants { get; set; } = new List<ChatParticipant>();
}