using Init.Application.Common.Interfaces;
using Init.Domain.AIRecommendations;

namespace Init.Application.AIRecommendations.Commands.CreateAIRecommendation;

public class CreateAIRecommendationCommandHandler(
    IAIRecommendationsRepository repository, IUnitOfWork unitOfWork) 
    : IRequestHandler<CreateAIRecommendationCommand, ErrorOr<AIRecommendation>>
{
    public async Task<ErrorOr<AIRecommendation>> Handle(
        CreateAIRecommendationCommand request, 
        CancellationToken cancellationToken)
    {
        var aiRecommendation = new AIRecommendation
        {
            Year = request.ReportDate.Year,
            Month = request.ReportDate.Month,
            ReportDate = request.ReportDate,
            RecommendationsHtml = request.Recommendations,
        };
        
        await repository.AddAsync(aiRecommendation, cancellationToken);

        await unitOfWork.CommitChangesAsync(cancellationToken);
        
        return aiRecommendation;
    }
}