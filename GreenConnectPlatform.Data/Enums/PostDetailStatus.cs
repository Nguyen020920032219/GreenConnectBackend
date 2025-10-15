namespace GreenConnectPlatform.Data.Enums;

public enum PostDetailStatus
{
    /// <summary>
    ///     Món hàng đang có sẵn để đặt.
    /// </summary>
    Available,

    /// <summary>
    ///     Món hàng đã được một người thu mua đặt trước.
    /// </summary>
    Booked,

    /// <summary>
    ///     Món hàng đã được thu gom thành công.
    /// </summary>
    Collected
}