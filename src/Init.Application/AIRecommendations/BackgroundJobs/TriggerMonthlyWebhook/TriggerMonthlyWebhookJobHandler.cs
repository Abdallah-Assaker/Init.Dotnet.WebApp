using HangfireJobsKit.Handlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using Init.Application.Common.Constants;

namespace Init.Application.AIRecommendations.BackgroundJobs.TriggerMonthlyWebhook;

public class TriggerMonthlyWebhookJobHandler(
    ILogger<TriggerMonthlyWebhookJobHandler> logger, 
    IConfiguration configuration,
    IHttpClientFactory httpClientFactory)
    : DelayedJobHandlerBase<TriggerMonthlyWebhookJob>
{
    
    private const string WebhookUrl = "/api/iwh/4874fbf7xxxxxxxxxxxxxxx17941a81";
    
    protected override async Task Handle(TriggerMonthlyWebhookJob job)
    {
        try
        {
            var body = new
            {
                API = configuration[AppSettingsMapping.AppSettingsServerUrl],
                Email = "temp.mail@gmail.com",
                Date = DateTime.Now.ToString("yyyy-MM-01"),
            };
            
            var client = httpClientFactory.CreateClient("UChatWebhook");
            
            await client.PostAsJsonAsync(WebhookUrl, body);
            
            logger.LogInformation("Webhook triggered successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error triggering webhook");
            throw; 
        }
    }
}