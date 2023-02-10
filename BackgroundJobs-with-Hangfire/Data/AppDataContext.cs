using BackgroundJobs_with_Hangfire.Models;
using Microsoft.EntityFrameworkCore;

namespace BackgroundJobs_with_Hangfire.Data;
public class AppDataContext : DbContext
{
    public AppDataContext(DbContextOptions options) : base(options) { }

    public DbSet<Driver> Drivers { get; set; }
}
