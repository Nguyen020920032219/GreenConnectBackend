namespace GreenConnectPlatform.Data.Entities;

public class UserPackage
{
    public Guid UserPackageId { get; set; }
    public Guid UserId { get; set; }
    public Guid PackageId { get; set; }
    public DateTime ActivationDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public int? RemainingConnections { get; set; }

    public virtual User User { get; set; } = null!;
    public virtual PaymentPackage Package { get; set; } = null!;
}