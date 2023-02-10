namespace BackgroundJobs_with_Hangfire.Services.Interfaces;

public interface IServiceManagement
{
    void SendMail();
    void UpdateDatabase();
    void SyncData();
}
