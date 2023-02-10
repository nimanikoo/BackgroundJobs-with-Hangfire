namespace BackgroundJobs_with_Hangfire.Models;

public class Driver
{
    public Guid Id { get; set; }
    public int DriverNumber { get; set; }
    public int Status { get; set; }
    public string Name { get; set; }
}
