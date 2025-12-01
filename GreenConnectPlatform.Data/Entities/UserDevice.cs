using System.ComponentModel.DataAnnotations;

namespace GreenConnectPlatform.Data.Entities;

public class UserDevice
{
    [Key] public Guid DeviceId { get; set; }

    public Guid UserId { get; set; }

    public string FcmToken { get; set; } = null!; // Token nhận từ Firebase Client

    public string? Platform { get; set; } // android, ios, web

    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

    public virtual User? User { get; set; }
}