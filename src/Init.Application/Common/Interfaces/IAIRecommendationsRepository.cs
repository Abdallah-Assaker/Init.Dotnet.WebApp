using Init.Domain.AIRecommendations;

namespace Init.Application.Common.Interfaces;

public interface IAIRecommendationsRepository
{
    Task<AIRecommendation> AddAsync(AIRecommendation recommendation, CancellationToken cancellationToken = default);
    Task<List<AIRecommendation>> GetRecentRecommendationsAsync(DateOnly fromDate, DateOnly toDate, CancellationToken cancellationToken = default);
}