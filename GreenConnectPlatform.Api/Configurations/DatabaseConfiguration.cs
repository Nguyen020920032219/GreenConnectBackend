using System.Configuration;
using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Npgsql;
using ConfigurationManager = Microsoft.Extensions.Configuration.ConfigurationManager;

namespace GreenConnectPlatform.Api.Configurations;

public static class DatabaseConfiguration
{
    public static async Task AddPostgresAsync(this IServiceCollection services,
        ConfigurationManager configuration,
        string connectionName,
        bool isDevelopment = false)
    {
        var props = GetDbConnectionProps(configuration, connectionName);
        if (string.IsNullOrWhiteSpace(props.DbHost))
            throw new ConfigurationErrorsException("Missing DbHost for database configuration");

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

        services.AddDbContext<GreenConnectDbContext>(options =>
        {
            options.UseNpgsql(
                props.PsqlConnectionString,
                npgsqlOptions => npgsqlOptions.UseNetTopologySuite()
            );

            if (isDevelopment)
                options.EnableSensitiveDataLogging();

            options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
        });

        await CreateDbIfNotExistsAsync(props);
    }

    private static async Task CreateDbIfNotExistsAsync(DbConnectionProps props)
    {
        var dbName = props.Database;
        if (string.IsNullOrWhiteSpace(dbName))
            throw new ConfigurationErrorsException("Missing database name for database configuration");

        var connectionBuilder = new NpgsqlConnectionStringBuilder
        {
            Host = props.DbHost,
            Port = props.DbPort,
            Username = props.DbUser,
            Password = props.DbPass
        };

        await using var conn = new NpgsqlConnection(connectionBuilder.ToString());
        await using var command = new NpgsqlCommand("SELECT 1 FROM pg_catalog.pg_database WHERE datname = @name", conn);
        command.Parameters.AddWithValue("@name", dbName);

        await conn.OpenAsync();
        var result = await command.ExecuteScalarAsync();
        if (result?.ToString() != "1")
        {
            Console.WriteLine($"[Database] Database '{dbName}' not found. Creating...");
            await using var createDbCmd = new NpgsqlCommand($"CREATE DATABASE \"{dbName}\"", conn);
            await createDbCmd.ExecuteNonQueryAsync();
            Console.WriteLine($"[Database] Database '{dbName}' created successfully.");
        }
    }

    private static DbConnectionProps GetDbConnectionProps(ConfigurationManager configuration, string connectionName)
    {
        var section = configuration.GetSection("DatabaseConnection").GetSection(connectionName);
        var props = new DbConnectionProps();
        section.Bind(props);

        props.DbHost = configuration[$"DatabaseConnection:{connectionName}:DbHost"] ?? props.DbHost;
        props.DbPort = int.TryParse(configuration[$"DatabaseConnection:{connectionName}:DbPort"], out var port)
            ? port
            : props.DbPort;

        return props;
    }

    [Obsolete("Obsolete")]
    public static void MapPostgresEnums()
    {
        NpgsqlConnection.GlobalTypeMapper.MapEnum<UserStatus>("user_status");
        NpgsqlConnection.GlobalTypeMapper.MapEnum<PostStatus>("post_status");
        NpgsqlConnection.GlobalTypeMapper.MapEnum<PostDetailStatus>("post_detail_status");
        NpgsqlConnection.GlobalTypeMapper.MapEnum<OfferStatus>("offer_status");
        NpgsqlConnection.GlobalTypeMapper.MapEnum<TransactionStatus>("transaction_status");
        NpgsqlConnection.GlobalTypeMapper.MapEnum<ComplaintStatus>("complaint_status");
        NpgsqlConnection.GlobalTypeMapper.MapEnum<ProposalStatus>("proposal_status");
        NpgsqlConnection.GlobalTypeMapper.MapEnum<VerificationStatus>("verification_status");
        NpgsqlConnection.GlobalTypeMapper.MapEnum<Gender>("gender");
        NpgsqlConnection.GlobalTypeMapper.MapEnum<PackageType>("package_type");
        NpgsqlConnection.GlobalTypeMapper.MapEnum<PaymentStatus>("payment_status");
        NpgsqlConnection.GlobalTypeMapper.MapEnum<BuyerType>("buyer_type");
        NpgsqlConnection.GlobalTypeMapper.MapEnum<ItemTransactionType>("item_transaction_type");
        NpgsqlConnection.GlobalTypeMapper.MapEnum<TransactionPaymentMethod>("transaction_payment_method");
        Console.WriteLine("[Database] PostgreSQL enums mapped successfully.");
    }
}