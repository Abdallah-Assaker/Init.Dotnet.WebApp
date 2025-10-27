using Init.Domain.AIRecommendations;

namespace Init.Application.AIRecommendations.Queries.GetRecentRecommendations;

public record GetRecentRecommendationsQuery(DateOnly FromDate, DateOnly ToDate) : IRequest<List<AIRecommendation>?>;