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
    public int PointBalance { get; set; } = 200;
    public int RankId { get; set; }
    public Point? Location { get; set; }
    public string? BankCode { get; set; }
    public string? BankAccountNumber { get; set; }
    public string? BankAccountName { get; set; }
    public int CreditBalance { get; set; } = 0;

    public virtual User? User { get; set; }
    public virtual Rank? Rank { get; set; }
}