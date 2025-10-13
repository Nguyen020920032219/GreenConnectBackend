using GreenConnectPlatform.Bussiness.Services.FileStorage;

namespace GreenConnectPlatform.Api.Configurations;

public static class ServiceConfiguration
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IFileStorageService, FirebaseStorageService>();
    }

    public static void ConfigureRepositories(this IServiceCollection services)
    {
        // Repositories
    }
}