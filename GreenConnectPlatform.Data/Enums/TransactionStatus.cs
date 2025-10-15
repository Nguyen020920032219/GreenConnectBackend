namespace GreenConnectPlatform.Data.Enums;

public enum TransactionStatus
{
    /// <summary>
    ///     Giao dịch đã được lên lịch hẹn.
    /// </summary>
    Scheduled,

    /// <summary>
    ///     Người thu mua đã check-in và đang trong quá trình thực hiện giao dịch.
    /// </summary>
    InProgress,

    /// <summary>
    ///     Giao dịch đã được cả hai bên xác nhận và hoàn tất.
    /// </summary>
    Completed,

    /// <summary>
    ///     Giao dịch bị hệ thống tự động hủy (ví dụ: quá giờ hẹn mà không check-in).
    /// </summary>
    CanceledBySystem,

    /// <summary>
    ///     Giao dịch bị một trong hai người dùng hủy.
    /// </summary>
    CanceledByUser
}