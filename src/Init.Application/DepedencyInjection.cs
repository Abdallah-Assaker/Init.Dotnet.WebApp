using HangfireJobsKit.Abstractions.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Init.Application.AIRecommendations.BackgroundJobs.TriggerMonthlyWebhook;
using Init.Application.Common.Interfaces;

namespace Init.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR()
            .RegisterBackgroundJobs()
            .RegisterJobsRegistrations();

        
        return services;
    }
    
    private static IServiceCollection RegisterBackgroundJobs(this IServiceCollection services)
    {
        services.AddScoped<IDelayedJobHandlerBase<TriggerMonthlyWebhookJob>, TriggerMonthlyWebhookJobHandler>();
        
        return services;
    }
    
    private static IServiceCollection RegisterJobsRegistrations(this IServiceCollection services)
    {
        return services;
    }
    
    private static IServiceCollection AddMediatR(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection));
        });
        
        return services;
    }
    
    public static IApplicationBuilder RegisterRecurringJobs(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        
        var recurringJobRegistrations = scope.ServiceProvider.GetServices<IRecurringJobRegistration>();
        foreach (var registration in recurringJobRegistrations)
        {
            registration.Register();
        }

        return app;
    }
}