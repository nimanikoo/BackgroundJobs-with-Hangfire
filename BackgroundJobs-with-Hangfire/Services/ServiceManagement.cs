using BackgroundJobs_with_Hangfire.Services.Interfaces;

namespace BackgroundJobs_with_Hangfire.Services;

public class ServiceManagement : IServiceManagement
{
    public void SendMail() => Console.WriteLine($"Sending mail Job start running: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

    public void SyncData() => Console.WriteLine($"Sync Data Job start running: {DateTime.Now:yyyy-MM-dd  HH:mm:ss}");

    public void UpdateDatabase() => Console.WriteLine($"Update Database Job start running: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
}