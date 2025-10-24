using GreenConnectPlatform.Business.Services.Auth;
using GreenConnectPlatform.Business.Services.FileStorage;
using GreenConnectPlatform.Bussiness.Mappers;
using GreenConnectPlatform.Bussiness.Services.ScrapPosts;
// using GreenConnectPlatform.Bussiness.Services.ScrapPosts.ScrapPostDetails;
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
        // services.AddScoped<IScrapPostDetailService, ScrapPostDetailService>();
    }

    public static void ConfigureRepositories(this IServiceCollection services)
    {
        // Repositories
        services.AddScoped<IScrapPostRepository, ScrapPostRepository>();
        services.AddScoped<IScrapPostDetailRepository, ScrapPostDetailRepository>();
        services.AddScoped<IScrapCategoryRepository, ScrapCategoryRepository>();
        services.AddScoped<IProfileRepository, ProfileRepository>();
    }
}