using GreenConnectPlatform.Business.Mappers;
using GreenConnectPlatform.Business.Services.Auth;
using GreenConnectPlatform.Business.Services.CollectionOffers;
using GreenConnectPlatform.Business.Services.FileStorage;
using GreenConnectPlatform.Business.Services.Jwt;
using GreenConnectPlatform.Business.Services.Profile;
using GreenConnectPlatform.Business.Services.ScheduleProposals;
using GreenConnectPlatform.Business.Services.ScrapCategories;
using GreenConnectPlatform.Business.Services.ScrapPosts;
using GreenConnectPlatform.Business.Services.Storage;
using GreenConnectPlatform.Business.Services.Transactions;
using GreenConnectPlatform.Data.Repositories.CollectionOffers;
using GreenConnectPlatform.Data.Repositories.CollectorVerificationInfos;
using GreenConnectPlatform.Data.Repositories.Profiles;
using GreenConnectPlatform.Data.Repositories.ScheduleProposals;
using GreenConnectPlatform.Data.Repositories.ScrapCategories;
using GreenConnectPlatform.Data.Repositories.ScrapPosts;
using GreenConnectPlatform.Data.Repositories.Transactions;

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
    }
}