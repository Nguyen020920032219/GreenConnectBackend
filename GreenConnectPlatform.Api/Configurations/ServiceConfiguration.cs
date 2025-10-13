using GreenConnectPlatform.Bussiness.Mappers;
using GreenConnectPlatform.Bussiness.Services.Auth;

namespace GreenConnectPlatform.Api.Configurations;

public static class ServiceConfiguration
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        // Services
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddScoped<IAuthService, AuthService>();
        services.AddSingleton<FirebaseService>();
    }

    public static void ConfigureRepositories(this IServiceCollection services)
    {
        // Repositories
    }
}