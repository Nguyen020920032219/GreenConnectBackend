namespace GreenConnectPlatform.Data.Enums;

public enum ItemTransactionType
{
    Sale = 0,       // Bán (Mặc định): Tiền dương
    Donation = 1,   // Cho tặng: Tiền = 0
    Service = 2     // Dịch vụ/Vứt bỏ: Tiền âm (người bán trả tiền)
}