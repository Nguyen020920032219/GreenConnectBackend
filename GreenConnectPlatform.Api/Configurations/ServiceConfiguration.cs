using GreenConnectPlatform.Business.Mappers;
using GreenConnectPlatform.Business.Services.Auth;
using GreenConnectPlatform.Business.Services.CollectionOffers;
using GreenConnectPlatform.Business.Services.CollectionOffers.OfferDetails;
using GreenConnectPlatform.Business.Services.FileStorage;
using GreenConnectPlatform.Business.Services.Jwt;
using GreenConnectPlatform.Business.Services.Profile;
using GreenConnectPlatform.Business.Services.ScheduleProposals;
using GreenConnectPlatform.Business.Services.ScrapCategories;
using GreenConnectPlatform.Business.Services.ScrapPosts;
using GreenConnectPlatform.Business.Services.ScrapPosts.ScrapPostDetails;
using GreenConnectPlatform.Business.Services.Transactions;
using GreenConnectPlatform.Business.Services.Transactions.TransactionDetails;
using GreenConnectPlatform.Data.Repositories.CollectionOffers;
using GreenConnectPlatform.Data.Repositories.CollectionOffers.OfferDetails;
using GreenConnectPlatform.Data.Repositories.CollectorVerificationInfos;
using GreenConnectPlatform.Data.Repositories.Profiles;
using GreenConnectPlatform.Data.Repositories.ScheduleProposals;
using GreenConnectPlatform.Data.Repositories.ScrapCategories;
using GreenConnectPlatform.Data.Repositories.ScrapPosts;
using GreenConnectPlatform.Data.Repositories.ScrapPosts.ScrapPostDetails;
using GreenConnectPlatform.Data.Repositories.Transactions;
using GreenConnectPlatform.Data.Repositories.Transactions.TransactionDetails;

namespace GreenConnectPlatform.Api.Configurations;

public static class ServiceConfiguration
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        // Services
        services.AddScoped<IScrapPostService, ScrapPostService>();
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddScoped<IAuthService, AuthService>();
        services.AddSingleton<IFileStorageService, FirebaseStorageService>();
        services.AddScoped<IScrapPostDetailService, ScrapPostDetailService>();
        services.AddScoped<ICollectionOfferService, CollectionOfferService>();
        services.AddScoped<IOfferDetailService, OfferDetailService>();
        services.AddScoped<IScheduleProposalService, ScheduleProposalService>();
        services.AddScoped<IScrapCategoryService, ScrapCategoryService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<ITransactionDetailService, TransactionDetailService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IProfileService, ProfileService>();

    }

    public static void ConfigureRepositories(this IServiceCollection services)
    {
        // Repositories
        services.AddScoped<IScrapPostRepository, ScrapPostRepository>();
        services.AddScoped<IScrapPostDetailRepository, ScrapPostDetailRepository>();
        services.AddScoped<IScrapCategoryRepository, ScrapCategoryRepository>();
        services.AddScoped<IProfileRepository, ProfileRepository>();
        services.AddScoped<ICollectionOfferRepository, CollectionOfferRepository>();
        services.AddScoped<IOfferDetailRepository, OfferDetailRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<ITransactionDetailRepository, TransactionDetailRepository>();
        services.AddScoped<IScheduleProposalRepository, ScheduleProposalRepository>();
        services.AddScoped<IProfileRepository, ProfileRepository>();
        services.AddScoped<IVerificationInfoRepository, VerificationInfoRepository>();
    }
}