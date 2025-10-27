namespace Init.Domain.AIRecommendations;

public class AIRecommendation
{
    public int Id { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public DateOnly ReportDate { get; set; }
    public string? RecommendationsHtml { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}