using GreenConnectPlatform.Data.Enums;
using NetTopologySuite.Geometries;

namespace GreenConnectPlatform.Data.Entities;

public class Profile
{
    public Guid ProfileId { get; set; }

    public Guid UserId { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Address { get; set; }

    public Gender? Gender { get; set; }

    public string? AvatarUrl { get; set; }

    public int PointBalance { get; set; } = 0;

    public int RankId { get; set; } = 1;

    public Point? Location { get; set; }

    public virtual User User { get; set; } = null!;
    public virtual Rank Rank { get; set; } = null!;
}