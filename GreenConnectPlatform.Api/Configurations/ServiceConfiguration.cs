using GreenConnectPlatform.Business.Mappers;
using GreenConnectPlatform.Business.Services.AI;
using GreenConnectPlatform.Business.Services.Auth;
using GreenConnectPlatform.Business.Services.Banks;
using GreenConnectPlatform.Business.Services.Chat;
using GreenConnectPlatform.Business.Services.CollectionOffers;
using GreenConnectPlatform.Business.Services.Complaints;
using GreenConnectPlatform.Business.Services.Feedbacks;
using GreenConnectPlatform.Business.Services.FileStorage;
using GreenConnectPlatform.Business.Services.Jwt;
using GreenConnectPlatform.Business.Services.Notifications;
using GreenConnectPlatform.Business.Services.Payment;
using GreenConnectPlatform.Business.Services.PaymentPackages;
using GreenConnectPlatform.Business.Services.PointHistories;
using GreenConnectPlatform.Business.Services.Profile;
using GreenConnectPlatform.Business.Services.ReferencePrices;
using GreenConnectPlatform.Business.Services.Reports;
using GreenConnectPlatform.Business.Services.RewardItems;
using GreenConnectPlatform.Business.Services.ScheduleProposals;
using GreenConnectPlatform.Business.Services.ScrapCategories;
using GreenConnectPlatform.Business.Services.ScrapPosts;
using GreenConnectPlatform.Business.Services.Storage;
using GreenConnectPlatform.Business.Services.Transactions;
using GreenConnectPlatform.Business.Services.Users;
using GreenConnectPlatform.Business.Services.VerificationInfos;
using GreenConnectPlatform.Data.Repositories.Chatrooms;
using GreenConnectPlatform.Data.Repositories.CollectionOffers;
using GreenConnectPlatform.Data.Repositories.CollectorVerificationInfos;
using GreenConnectPlatform.Data.Repositories.Complaints;
using GreenConnectPlatform.Data.Repositories.CreditTransactionHistories;
using GreenConnectPlatform.Data.Repositories.Feedbacks;
using GreenConnectPlatform.Data.Repositories.Messages;
using GreenConnectPlatform.Data.Repositories.Notifications;
using GreenConnectPlatform.Data.Repositories.PaymentPackages;
using GreenConnectPlatform.Data.Repositories.PaymentTransactions;
using GreenConnectPlatform.Data.Repositories.PointHistories;
using GreenConnectPlatform.Data.Repositories.Profiles;
using GreenConnectPlatform.Data.Repositories.ReferencePrices;
using GreenConnectPlatform.Data.Repositories.RewardItems;
using GreenConnectPlatform.Data.Repositories.ScheduleProposals;
using GreenConnectPlatform.Data.Repositories.ScrapCategories;
using GreenConnectPlatform.Data.Repositories.ScrapPosts;
using GreenConnectPlatform.Data.Repositories.Transactions;
using GreenConnectPlatform.Data.Repositories.UserDevices;
using GreenConnectPlatform.Data.Repositories.UserPackages;
using GreenConnectPlatform.Data.Repositories.Users;

namespace GreenConnectPlatform.Api.Configurations;

public static class ServiceConfiguration
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        // --- Services ---
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddScoped<IJwtService, JwtService>();
        services.AddSingleton<IFileStorageService, FirebaseStorageService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IStorageService, StorageService>();
        services.AddScoped<IScrapCategoryService, ScrapCategoryService>();
        services.AddScoped<IScrapPostService, ScrapPostService>();
        services.AddScoped<ICollectionOfferService, CollectionOfferService>();
        services.AddScoped<IScheduleProposalService, ScheduleProposalService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<IVerificationInfoService, VerificationInfoService>();
        services.AddScoped<IComplaintService, ComplaintService>();
        services.AddScoped<IPaymentPackageService, PaymentPackageService>();
        services.AddScoped<IReferencePriceService, ReferencePriceService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRewardItemService, RewardItemService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IFeedbackService, FeedbackService>();
        services.AddScoped<IChatService, ChatService>();
        services.AddHttpClient<IScrapRecognitionService, GeminiScrapRecognitionService>();
        services.AddScoped<IPointHistoryService, PointHistoryService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddHostedService<TransactionAutoCancelBackGroundService>();
        services.AddScoped<IVnPayService, VnPayService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IVietQrService, VietQrService>();
        services.AddHttpClient<IBankService, BankService>();
    }

    public static void ConfigureRepositories(this IServiceCollection services)
    {
        // --- Repositories ---
        services.AddScoped<IProfileRepository, ProfileRepository>();
        services.AddScoped<IVerificationInfoRepository, VerificationInfoRepository>();
        services.AddScoped<IScrapCategoryRepository, ScrapCategoryRepository>();
        services.AddScoped<IScrapPostRepository, ScrapPostRepository>();
        services.AddScoped<ICollectionOfferRepository, CollectionOfferRepository>();
        services.AddScoped<IScheduleProposalRepository, ScheduleProposalRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IVerificationInfoRepository, VerificationInfoRepository>();
        services.AddScoped<IComplaintRepository, ComplaintRepository>();
        services.AddScoped<IPaymentPackageRepository, PaymentPackageRepository>();
        services.AddScoped<IReferencePriceRepository, ReferencePriceRepository>();
        services.AddScoped<IUserPackageRepository, UserPackageRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRewardItemRepository, RewardItemRepository>();
        services.AddScoped<IFeedbackRepository, FeedbackRepository>();
        services.AddScoped<IChatRoomRepository, ChatRoomRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IPointHistoryRepository, PointHistoryRepository>();
        services.AddScoped<IUserDeviceRepository, UserDeviceRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<ICreditTransactionHistoryRepository, CreditTransactionHistoryRepository>();
        services.AddScoped<IPaymentTransactionRepository, PaymentTransactionRepository>();
    }
}