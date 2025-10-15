using NetTopologySuite.Geometries;

namespace GreenConnectPlatform.Data.Entities;

public class Profile
{
    public Guid ProfileId { get; set; }

    public Guid UserId { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Address { get; set; }

    public string? Gender { get; set; }

    public string? AvatarUrl { get; set; }

    public int RewardPoint { get; set; }

    public Point? Location { get; set; }

    public virtual User User { get; set; } = null!;
}