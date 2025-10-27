using Init.Application.Common.Interfaces;
using Init.Domain.AIRecommendations;

namespace Init.Application.AIRecommendations.Queries.GetRecentRecommendations;

public class GetRecentRecommendationsQueryHandler(
    IAIRecommendationsRepository repository)
    : IRequestHandler<GetRecentRecommendationsQuery, List<AIRecommendation>?>
{
    public async Task<List<AIRecommendation>?> Handle(
        GetRecentRecommendationsQuery request, 
        CancellationToken cancellationToken)
    {
        var recommendations = await repository.GetRecentRecommendationsAsync(
            request.FromDate,
            request.ToDate,
            cancellationToken);
        
        return recommendations;
    }
}