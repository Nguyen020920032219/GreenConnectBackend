using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Data.Entities;

public class ChatParticipant
{
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    public Guid UserId { get; set; }
    public Guid ChatRoomId { get; set; }
    public virtual User User { get; set; } = null!;
    public virtual ChatRoom ChatRoom { get; set; } = null!;
}