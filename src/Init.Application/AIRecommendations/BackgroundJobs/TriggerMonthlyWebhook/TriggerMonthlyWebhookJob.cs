using HangfireJobsKit.Abstractions;
using HangfireJobsKit.Configuration;

namespace Init.Application.AIRecommendations.BackgroundJobs.TriggerMonthlyWebhook;

[JobConfiguration("Trigger AI Monthly Generation Report Webhook", retryAttempts: 3, logEvents: true)]
public record TriggerMonthlyWebhookJob : IDelayedJob;