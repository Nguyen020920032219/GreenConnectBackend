using GreenConnectPlatform.Business.Models.Notifications;
using GreenConnectPlatform.Business.Models.Paging;

namespace GreenConnectPlatform.Business.Services.Notifications;

public interface INotificationService
{
    // Mobile gọi hàm này khi mở app để đăng ký Token
    Task RegisterDeviceAsync(Guid userId, RegisterDeviceRequest request);

    // Hàm nội bộ để các Service khác gọi (Offer, Transaction...)
    Task SendNotificationAsync(Guid userId, string title, string body, Dictionary<string, string>? data = null);

    // Lấy lịch sử thông báo
    Task<PaginatedResult<NotificationModel>> GetMyNotificationsAsync(Guid userId, int page, int size);

    // Đánh dấu đã đọc
    Task MarkAsReadAsync(Guid userId, Guid notificationId);
}