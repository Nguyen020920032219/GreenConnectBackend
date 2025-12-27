using AutoMapper;
using FirebaseAdmin.Messaging;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Notifications;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Notifications;
using GreenConnectPlatform.Data.Repositories.UserDevices;
using Microsoft.AspNetCore.Http;
using Notification = GreenConnectPlatform.Data.Entities.Notification;

namespace GreenConnectPlatform.Business.Services.Notifications;

public class NotificationService : INotificationService
{
    private readonly IMapper _mapper;
    private readonly INotificationRepository _notificationRepo;
    private readonly IUserDeviceRepository _userDeviceRepo;

    public NotificationService(
        INotificationRepository notificationRepo,
        IUserDeviceRepository userDeviceRepo,
        IMapper mapper)
    {
        _notificationRepo = notificationRepo;
        _userDeviceRepo = userDeviceRepo;
        _mapper = mapper;
    }

    public async Task RegisterDeviceAsync(Guid userId, RegisterDeviceRequest request)
    {
        // Kiểm tra xem Token này đã tồn tại chưa
        var existingDevice = await _userDeviceRepo.GetByTokenAsync(request.FcmToken);

        if (existingDevice != null)
        {
            // Nếu token đã tồn tại nhưng của user khác (trường hợp đăng xuất rồi đăng nhập nick khác trên cùng máy)
            // -> Update lại UserId cho token này
            if (existingDevice.UserId != userId) existingDevice.UserId = userId;
            existingDevice.LastUpdated = DateTime.Now;
            await _userDeviceRepo.UpdateAsync(existingDevice);
        }
        else
        {
            // Token mới -> Tạo mới
            var newDevice = new UserDevice
            {
                DeviceId = Guid.NewGuid(),
                UserId = userId,
                FcmToken = request.FcmToken,
                Platform = request.Platform,
                LastUpdated = DateTime.Now
            };
            await _userDeviceRepo.AddAsync(newDevice);
        }
    }

    public async Task SendNotificationAsync(Guid userId, string title, string body,
        Dictionary<string, string>? data = null)
    {
        // 1. Lưu vào Database trước (để user xem lại lịch sử)
        var notification = new Notification
        {
            NotificationId = Guid.NewGuid(),
            RecipientId = userId,
            Content = body, // Hoặc title + body
            IsRead = false,
            CreatedAt = DateTime.Now,
            // EntityType/EntityId có thể truyền qua data dictionary nếu cần
            EntityType = data?.ContainsKey("type") == true ? data["type"] : "System",
            EntityId = data?.ContainsKey("id") == true && Guid.TryParse(data["id"], out var id) ? id : null
        };
        await _notificationRepo.AddAsync(notification);

        // 2. Lấy danh sách Token của User
        var tokens = await _userDeviceRepo.GetTokensByUserIdAsync(userId);
        if (tokens.Count == 0) return; // User không online/không có thiết bị -> Chỉ lưu DB

        // 3. Gửi qua Firebase (Multicast)
        var message = new MulticastMessage
        {
            Tokens = tokens,
            Notification = new FirebaseAdmin.Messaging.Notification
            {
                Title = title,
                Body = body
            },
            Data = data // Dữ liệu để Mobile App xử lý khi click vào (VD: Mở màn hình Transaction)
        };

        try
        {
            await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
        }
        catch (Exception ex)
        {
            // Log lỗi gửi Firebase nhưng không throw exception để tránh làm hỏng luồng chính
            Console.WriteLine($"Lỗi gửi FCM: {ex.Message}");
        }
    }

    public async Task<PaginatedResult<NotificationModel>> GetMyNotificationsAsync(Guid userId, int page, int size)
    {
        var (items, total) = await _notificationRepo.GetByUserIdAsync(userId, page, size);

        return new PaginatedResult<NotificationModel>
        {
            Data = _mapper.Map<List<NotificationModel>>(items),
            Pagination = new PaginationModel(total, page, size)
        };
    }

    public async Task MarkAsReadAsync(Guid userId, Guid notificationId)
    {
        var noti = await _notificationRepo.GetByIdAsync(notificationId);
        if (noti == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Không tìm thấy thông báo.");

        if (noti.RecipientId != userId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "Không có quyền.");

        noti.IsRead = true;
        await _notificationRepo.UpdateAsync(noti);
    }
}