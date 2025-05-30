namespace Microsoft.Teams.Assist.Infrastructure.BackgroundJobs;
public class HangfireStorageSettings
{
    public string? StorageProvider { get; set; }
    public string? ConnectionString { get; set; }
}