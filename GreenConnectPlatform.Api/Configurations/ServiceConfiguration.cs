using GreenConnectPlatform.Business.Mappers;
using GreenConnectPlatform.Business.Services.Auth;
using GreenConnectPlatform.Business.Services.FileStorage;
using GreenConnectPlatform.Business.Services.Jwt;
using GreenConnectPlatform.Business.Services.Profile;
using GreenConnectPlatform.Business.Services.ScrapPosts;
using GreenConnectPlatform.Business.Services.ScrapPosts.ScrapPostDetails;
using GreenConnectPlatform.Data.Repositories.CollectorVerificationInfos;
using GreenConnectPlatform.Data.Repositories.Profiles;
using GreenConnectPlatform.Data.Repositories.ScrapCategories;
using GreenConnectPlatform.Data.Repositories.ScrapPosts;
using GreenConnectPlatform.Data.Repositories.ScrapPosts.ScrapPostDetails;

namespace GreenConnectPlatform.Api.Configurations;

public static class ServiceConfiguration
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        // Services
        services.AddScoped<IScrapPostService, ScrapPostService>();
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddScoped<IAuthService, AuthService>();
        services.AddSingleton<FirebaseService>();
        services.AddSingleton<IFileStorageService, FirebaseStorageService>();
        services.AddScoped<IScrapPostDetailService, ScrapPostDetailService>();
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
        services.AddScoped<IVerificationInfoRepository, VerificationInfoRepository>();
    }
}