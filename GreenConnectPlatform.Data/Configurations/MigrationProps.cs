namespace GreenConnectPlatform.Data.Configurations;

public class MigrationProps
{
    public bool IsEnable { get; set; }
    public int RetryCount { get; set; }
    public int RetryDelay { get; set; }
    public string ScriptPath { get; set; } = null!;
}