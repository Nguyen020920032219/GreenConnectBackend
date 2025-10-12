using GreenConnectPlatform.Api.Configurations;
using GreenConnectPlatform.Data.Enums;
using Npgsql;

namespace GreenConnectPlatform.Api;

public class Program
{
    [Obsolete("Obsolete")]
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.ConfigureAuthentication(builder.Configuration);

        builder.Services.ConfigureSwagger();

        builder.Services.AddAuthorization();

        await builder.Services.AddPostgresAsync(builder.Configuration, "DefaultConnection",
            builder.Environment.IsDevelopment());

        NpgsqlConnection.GlobalTypeMapper.MapEnum<UserStatus>("user_status");
        NpgsqlConnection.GlobalTypeMapper.MapEnum<PostStatus>("post_status");
        NpgsqlConnection.GlobalTypeMapper.MapEnum<ItemStatus>("item_status");
        NpgsqlConnection.GlobalTypeMapper.MapEnum<OfferStatus>("offer_status");
        NpgsqlConnection.GlobalTypeMapper.MapEnum<TransactionStatus>("transaction_status");
        NpgsqlConnection.GlobalTypeMapper.MapEnum<ComplaintStatus>("complaint_status");
        
        builder.Services.ConfigureServices();

        builder.Services.ConfigureRepositories();

        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

        builder.Services.AddProblemDetails();

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

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