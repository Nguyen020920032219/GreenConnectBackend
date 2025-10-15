namespace GreenConnectPlatform.Data.Enums;

public enum UserStatus
{
    /// <summary>
    ///     Tài khoản đang chờ được quản trị viên xác minh (đặc biệt cho Scrap Collector).
    /// </summary>
    PendingVerification,

    /// <summary>
    ///     Tài khoản đang hoạt động bình thường.
    /// </summary>
    Active,

    /// <summary>
    ///     Tài khoản không hoạt động.
    /// </summary>
    Inactive,

    /// <summary>
    ///     Tài khoản đã bị quản trị viên khóa.
    /// </summary>
    Blocked
}