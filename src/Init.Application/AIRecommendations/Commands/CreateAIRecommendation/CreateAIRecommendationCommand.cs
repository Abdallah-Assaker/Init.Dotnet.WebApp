using Init.Domain.AIRecommendations;

namespace Init.Application.AIRecommendations.Commands.CreateAIRecommendation;

public record CreateAIRecommendationCommand(string Recommendations, DateOnly ReportDate) : IRequest<ErrorOr<AIRecommendation>>;