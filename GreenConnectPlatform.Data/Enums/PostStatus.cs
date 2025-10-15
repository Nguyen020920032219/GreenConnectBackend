namespace GreenConnectPlatform.Data.Enums;

public enum PostStatus
{
    /// <summary>
    ///     Bài đăng đang mở và sẵn sàng nhận đề nghị.
    /// </summary>
    Open,

    /// <summary>
    ///     Một phần các món hàng trong bài đăng đã được đặt.
    /// </summary>
    PartiallyBooked,

    /// <summary>
    ///     Tất cả các món hàng trong bài đăng đã được đặt.
    /// </summary>
    FullyBooked,

    /// <summary>
    ///     Toàn bộ giao dịch cho bài đăng đã hoàn tất.
    /// </summary>
    Completed,

    /// <summary>
    ///     Bài đăng đã bị hủy bởi người dùng.
    /// </summary>
    Canceled
}