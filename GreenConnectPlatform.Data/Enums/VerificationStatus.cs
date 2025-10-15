namespace GreenConnectPlatform.Data.Enums;

public enum VerificationStatus
{
    /// <summary>
    ///     Người dùng chưa gửi thông tin xác thực.
    /// </summary>
    NotSubmitted,

    /// <summary>
    ///     Thông tin đã được gửi và đang chờ quản trị viên duyệt.
    /// </summary>
    PendingReview,

    /// <summary>
    ///     Thông tin đã được phê duyệt.
    /// </summary>
    Approved,

    /// <summary>
    ///     Thông tin đã bị từ chối.
    /// </summary>
    Rejected
}