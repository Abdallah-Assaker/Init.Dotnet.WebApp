using System.Diagnostics;
using Hangfire;
using HangfireJobsKit.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;
using Init.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Init.Infrastructure.AIRecommendations.Persistence;
using Init.Infrastructure.Common.Services;
using Init.Infrastructure.Common.Persistence;
using Microsoft.Extensions.Logging;
using Init.Application.Common.Constants;
using Init.Infrastructure.Common.Hangfire;
using Serilog;
using Serilog.Events;
using Init.Application.Common.Configuration;

namespace Init.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);
        services.AddBackgroundServices(configuration);
        services.RegisterSqlConnectionForSpExec(configuration);
        services.AddCommonServices(configuration);
        services.AddSerilogLogging(configuration);
        
        return services;
    }
    
    public static IApplicationBuilder UseHangfireJobsKitMiddleware(this IApplicationBuilder app, IConfiguration configuration)
    {
        // Get dashboard credentials from configuration
        var username = configuration[AppSettingsMapping.HangfireUserName] ?? "admin";
        var password = configuration[AppSettingsMapping.HangfirePassword] ?? "admin";

        app.UseHangfireDashboard("/hangfire", new DashboardOptions
        {
            Authorization =
            [
                new HangfireBasicAuthenticationFilter(username, password)
            ],
            DashboardTitle = "Init Analytics Jobs"
        });
        
        app.UseHangfireJobsKit();
        
        return app;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                .LogTo(log => Debug.WriteLine(log), Microsoft.Extensions.Logging.LogLevel.Information)
                .EnableSensitiveDataLogging());

        services.AddScoped<IAIRecommendationsRepository, AIRecommendationsRepository>();
        services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<AppDbContext>());

        return services;
    }
    
    private static IServiceCollection AddBackgroundServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfireWithJobsKit(
            configuration.GetConnectionString("DefaultConnection")!);
        
        services.AddHangfireJobsKitServer(
            "V2Server", ["default"]);
        
        return services;
    }

    private static IServiceCollection RegisterSqlConnectionForSpExec(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<System.Data.IDbConnection>(_ => 
            new SqlConnection(configuration.GetConnectionString("DefaultConnection")));
        
        return services;
    }
    
    private static IServiceCollection AddCommonServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IEmailService, EmailService>();
        services.Configure<EmailConfiguration>(configuration.GetSection("EmailSettings"));
        services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
        
        // Register HttpClient for webhook communication
        services.AddHttpClient("UChatWebhook", client =>
        {
            client.BaseAddress = new Uri("https://www.uchat.com.au/");
        });
        
        return services;
    }
    
    private static IServiceCollection AddSerilogLogging(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File(
                path: "logs/Init-analytics-.log",
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 30,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .CreateLogger();

        // Add Serilog to services
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(dispose: true);
        });
        
        return services;
    }
}