using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Data.Entities;

public class Profile
{
    public Guid ProfileId { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Address { get; set; }
    public string? Gender { get; set; }
    public string? AvatarUrl { get; set; }
    public int RewardPoint { get; set; } = 0;
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = null!;
}