namespace Init.Contracts.AIRecommendations.GetAIRecommendation;

public record GetAIRecommendationRequest(
    DateOnly FromDate,
    DateOnly ToDate);