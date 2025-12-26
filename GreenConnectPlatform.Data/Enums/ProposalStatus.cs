namespace GreenConnectPlatform.Data.Enums;

public enum ProposalStatus
{
    /// <summary>
    ///     Đề xuất đang chờ phản hồi từ bên kia.
    /// </summary>
    Pending,

    /// <summary>
    ///     Đề xuất đã được chấp nhận.
    /// </summary>
    Accepted,

    /// <summary>
    ///     Đề xuất đã bị từ chối.
    /// </summary>
    Rejected,

    /// <summary>
    ///     Đề xuất đã bị người gửi hủy.
    /// </summary>
    Canceled
}