using GreenConnectPlatform.Bussiness.Mappers;
using GreenConnectPlatform.Bussiness.Services.Auth;
using GreenConnectPlatform.Bussiness.Services.FileStorage;
using GreenConnectPlatform.Bussiness.Services.ScrapPosts;
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
    }

    public static void ConfigureRepositories(this IServiceCollection services)
    {
        // Repositories
        services.AddScoped<IScrapPostRepository, ScrapPostRepository>();
        services.AddScoped<IScrapPostDetailRepository, ScrapPostDetailRepository>();
        services.AddScoped<IScrapCategoryRepository, ScrapCategoryRepository>();
    }
}