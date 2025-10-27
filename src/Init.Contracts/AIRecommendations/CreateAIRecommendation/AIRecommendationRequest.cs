namespace Init.Contracts.AIRecommendations.CreateAIRecommendation;

public record AIRecommendationRequest(
    string Recommendations,
    DateOnly? Date);