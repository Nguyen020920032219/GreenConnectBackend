using System.Text.Json.Serialization;
using DotNetEnv;
using GreenConnectPlatform.Api.Configurations;
using GreenConnectPlatform.Business.Hubs;
using GreenConnectPlatform.Data.Configurations;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        Env.Load();
        var builder = WebApplication.CreateBuilder(args);
        builder.Configuration
            .AddJsonFile(Path.Combine(builder.Environment.ContentRootPath, "Configs", "firebase-service-account.json"),
                true, true)
            .AddEnvironmentVariables();

        var allowedOrigins = (builder.Configuration["CORS:AllowedOrigins"] ?? "")
            .Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        builder.Services.AddSignalR();

        builder.Services.AddCors(opt =>
        {
            opt.AddPolicy("Default", p =>
            {
                if (allowedOrigins.Length > 0)
                    p.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                else
                    p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            });
        });

        var disableFirebase = builder.Configuration.GetValue<bool>("Testing:DisableFirebase");

        builder.Services.ConfigureAuthentication(builder.Configuration);
        if (!disableFirebase) builder.Services.ConfigureFirebase(builder.Environment);
        builder.Services.ConfigureSwagger();
        builder.Services.AddAuthorization();

        DatabaseConfiguration.MapPostgresEnums();

        await builder.Services.AddPostgresAsync(builder.Configuration, "DefaultConnection",
            builder.Environment.IsDevelopment());

        builder.Services.ConfigureServices();
        builder.Services.ConfigureRepositories();

        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();

        builder.Services.AddMemoryCache();

        builder.Services.AddControllers()
            .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        builder.Services.AddOpenApi();

        var keysPath = builder.Configuration["DATAPROTECTION_KEYS_PATH"];
        if (!string.IsNullOrWhiteSpace(keysPath))
            builder.Services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(keysPath))
                .SetApplicationName("GreenConnectPlatform");

        builder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();
        });

        var app = builder.Build();

        try
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<GreenConnectDbContext>();

            Console.WriteLine("[Database] Applying EF Core migrations...");
            await dbContext.Database.ExecuteSqlRawAsync("CREATE EXTENSION IF NOT EXISTS postgis;");
            await dbContext.Database.MigrateAsync();
            Console.WriteLine("[Database] EF Core migrations applied successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Database] Migration failed: {ex.Message}");
            throw;
        }

        app.UseForwardedHeaders();

        if (app.Environment.IsProduction())
        {
            app.UseHsts();
            if (app.Configuration.GetValue<bool>("EnableHttpsRedirect"))
                app.UseHttpsRedirection();
        }

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseExceptionHandler();

        app.UseCors("Default");

        app.UseAuthentication();
        app.UseAuthorization();
        app.MapHub<ChatHub>("/chatHub");
        app.MapGet("/healthz", () => Results.Ok("ok")).AllowAnonymous();

        app.MapControllers();

        app.Run();
    }
}