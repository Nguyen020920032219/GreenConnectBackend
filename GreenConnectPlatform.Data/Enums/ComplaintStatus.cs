namespace GreenConnectPlatform.Data.Enums;

public enum ComplaintStatus
{
    /// <summary>
    ///     Khiếu nại vừa được gửi và đang chờ quản trị viên xem xét.
    /// </summary>
    Submitted,

    /// <summary>
    ///     Quản trị viên đang trong quá trình xem xét khiếu nại.
    /// </summary>
    InReview,

    /// <summary>
    ///     Khiếu nại đã được giải quyết.
    /// </summary>
    Resolved,

    /// <summary>
    ///     Khiếu nại đã bị quản trị viên bác bỏ.
    /// </summary>
    Dismissed
}