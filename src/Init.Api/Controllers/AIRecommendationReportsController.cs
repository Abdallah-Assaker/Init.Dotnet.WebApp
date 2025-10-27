using HangfireJobsKit.Abstractions;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Init.Application.AIRecommendations.BackgroundJobs.TriggerMonthlyWebhook;
using Init.Application.AIRecommendations.Commands.CreateAIRecommendation;
using Init.Application.AIRecommendations.Queries.GetRecentRecommendations;
using Init.Contracts.AIRecommendations.CreateAIRecommendation;
using Init.Contracts.AIRecommendations.GetAIRecommendation;

namespace Init.Api.Controllers;

[Route("api/v2/[controller]")]
public class AIRecommendationReportsController(IMediator mediator) 
    : ApiController
{
    [HttpPost("trigger-monthly-webhook")]
    public IActionResult TriggerMonthlyWebhook([FromServices]IDelayedJobManager jobManager)
    {
        jobManager.Enqueue(new TriggerMonthlyWebhookJob());
        return Ok();
    }
    
    [HttpPost("recommendations")]
    public async Task<IActionResult> PostRecommendations([FromBody] AIRecommendationRequest request)
    {
        var command = new CreateAIRecommendationCommand(request.Recommendations, request.Date.GetValueOrDefault());
        
        var result = await mediator.Send(command);

        return result.Match(_ => Created(), Problem);
    }
    
    [HttpGet("recommendations")]
    public async Task<IActionResult> GetRecommendations([FromQuery] GetAIRecommendationRequest request)
    {
        var query = new GetRecentRecommendationsQuery(request.FromDate, request.ToDate);
        var result = await mediator.Send(query);
        
        return Ok(result?.Select(r => r.Adapt<GetAIRecommendationResponse>()));
    }
}