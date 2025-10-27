namespace Init.Contracts.AIRecommendations.GetAIRecommendation;

public record GetAIRecommendationResponse(
    int Year,
    int Month,
    DateOnly ReportDate,
    string? RecommendationsHtml);