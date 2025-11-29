namespace GreenConnectPlatform.Data.Entities;

public class CreditTransactionHistory
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }

    public int Amount { get; set; } // Số lượng thay đổi (VD: +50, -1)
    public int BalanceAfter { get; set; } // Số dư sau khi thay đổi

    public string Type { get; set; } = string.Empty; // Enum: "Purchase", "Usage", "Refund", "Bonus"

    public Guid? ReferenceId { get; set; } // ID tham chiếu (PaymentId, OfferId...)
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Property
    public virtual User? User { get; set; }
}