using System.Text.Json.Serialization;
using DotNetEnv;
using GreenConnectPlatform.Api.Configurations;

namespace GreenConnectPlatform.Api;

public class Program
{
    [Obsolete("Obsolete")]
    public static async Task Main(string[] args)
    {
        Env.Load();

        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddJsonFile(
            Path.Combine(builder.Environment.ContentRootPath, "Configs", "firebase-service-account.json"),
            true,
            true
        );

        // Add services to the container.
        builder.Services.ConfigureAuthentication(builder.Configuration);

        builder.Services.ConfigureFirebase(builder.Environment);

        builder.Services.ConfigureSwagger();

        builder.Services.AddAuthorization();

        await builder.Services.AddPostgresAsync(builder.Configuration, "DefaultConnection",
            builder.Environment.IsDevelopment());

        DatabaseConfiguration.MapPostgresEnums();

        builder.Services.ConfigureServices();

        builder.Services.ConfigureRepositories();

        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

        builder.Services.AddProblemDetails();
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        builder.Services.AddOpenApi();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            await DataSeeder.SeedRolesAsync(scope.ServiceProvider);
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment()) app.MapOpenApi();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseExceptionHandler();

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}