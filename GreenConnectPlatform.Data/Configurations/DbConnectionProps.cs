using Npgsql;

namespace GreenConnectPlatform.Data.Configurations;

public class DbConnectionProps
{
    public string? DbHost { get; set; }
    public int DbPort { get; set; }
    public string? DbUser { get; set; }
    public string? DbPass { get; set; }
    public string? Database { get; set; }

    public string PsqlConnectionString
    {
        get
        {
            var connectionBuilder = new NpgsqlConnectionStringBuilder
            {
                Host = DbHost,
                Port = DbPort,
                Username = DbUser,
                Password = DbPass,
                Database = Database
            };
            return connectionBuilder.ToString();
        }
    }
}