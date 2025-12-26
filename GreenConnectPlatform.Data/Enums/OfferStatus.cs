namespace GreenConnectPlatform.Data.Enums;

public enum OfferStatus
{
    /// <summary>
    ///     Đề nghị đang chờ hộ gia đình phản hồi.
    /// </summary>
    Pending,

    /// <summary>
    ///     Đề nghị đã được hộ gia đình chấp nhận.
    /// </summary>
    Accepted,

    /// <summary>
    ///     Đề nghị đã bị hộ gia đình từ chối.
    /// </summary>
    Rejected,

    /// <summary>
    ///     Đề nghị đã bị người thu mua hủy trước khi được chấp nhận.
    /// </summary>
    Canceled
}