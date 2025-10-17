using System.Text;
using System.Text.Json;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace GreenConnectPlatform.Api.Configurations;

public static class AuthConfiguration
{
    public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<User, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<GreenConnectDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        var jwtSettings = configuration.GetSection("Jwt");
        var secretKey = jwtSettings["Key"];
        if (string.IsNullOrEmpty(secretKey))
            throw new InvalidOperationException("Missing JWT secret key in configuration");

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,

                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),

                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();

                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";

                        var errorResponse = new
                        {
                            errorCode = "PR40101",
                            message = "Authentication error. A valid token is required to access this resource."
                        };

                        await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
                    },

                    OnForbidden = async context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        context.Response.ContentType = "application/json";

                        var errorResponse = new
                        {
                            errorCode = "PR40301",
                            message = "Not authorized. You do not have permission to access this resource."
                        };

                        await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
                    }
                };
            });
    }

    public static void ConfigureFirebase(this IServiceCollection services, IWebHostEnvironment environment)
    {
        var firebasePath = Path.Combine(environment.ContentRootPath, "Configs", "firebase-service-account.json");

        if (!File.Exists(firebasePath))
            throw new FileNotFoundException("Firebase config file not found.", firebasePath);

        if (FirebaseApp.DefaultInstance == null)
        {
            var credential = GoogleCredential.FromFile(firebasePath);
            FirebaseApp.Create(new AppOptions
            {
                Credential = credential
            });
        }

        Console.WriteLine($"Firebase App '{FirebaseApp.DefaultInstance?.Name}' initialized.");
    }
}