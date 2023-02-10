
using BackgroundJobs_with_Hangfire.Data;
using BackgroundJobs_with_Hangfire.Services;
using BackgroundJobs_with_Hangfire.Services.Interfaces;
using Hangfire;
using HangfireBasicAuthenticationFilter;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BackgroundJobs_with_Hangfire;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddTransient<IServiceManagement, ServiceManagement>();
        //Config Hangfire
        builder.Services.AddHangfire(config => config
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"))
        );
        builder.Services.AddHangfireServer();

        //Add DataBase to container
        builder.Services.AddDbContext<AppDataContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.UseHangfireDashboard("/hangfire", new DashboardOptions
        {
            DashboardTitle = " Hangfire BackgroundJobs Dashboard",
            Authorization = new[]
            {
                new HangfireCustomBasicAuthenticationFilter()
                {
                    Pass = "nimapass",
                    User = "nimanikoo"
                }
            }
        });
        app.MapHangfireDashboard();

        RecurringJob.AddOrUpdate<IServiceManagement>(s =>
        s.SyncData(), "* * * * * ");

        app.Run();
    }
}