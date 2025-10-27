using Microsoft.EntityFrameworkCore;
using Init.Application.Common.Interfaces;
using Init.Domain.AIRecommendations;
using Init.Infrastructure.Common.Persistence;

namespace Init.Infrastructure.AIRecommendations.Persistence;

public class AIRecommendationsRepository(AppDbContext dbContext) : IAIRecommendationsRepository
{
    public async Task<AIRecommendation> AddAsync(AIRecommendation recommendation, CancellationToken cancellationToken = default)
    {
        var entry = await dbContext.AIRecommendations.AddAsync(recommendation, cancellationToken);
        return entry.Entity;
    }

    public async Task<List<AIRecommendation>> GetRecentRecommendationsAsync(DateOnly fromDate, DateOnly toDate, CancellationToken cancellationToken = default)
    {
        return await dbContext.AIRecommendations
            .AsNoTracking()
            .Where(r => r.ReportDate >= fromDate && r.ReportDate <= toDate)
            .OrderByDescending(r => r.Year)
            .ThenByDescending(r => r.Month)
            .ToListAsync(cancellationToken);
    }
}