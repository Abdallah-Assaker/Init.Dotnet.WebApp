using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Init.Domain.AIRecommendations;

namespace Init.Infrastructure.AIRecommendations.Persistence;

public class AIRecommendationsConfigurations : IEntityTypeConfiguration<AIRecommendation>
{
    public void Configure(EntityTypeBuilder<AIRecommendation> builder)
    {
        builder.ToTable("AIRecommendations");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Year).IsRequired();
        builder.Property(x => x.Month).IsRequired();
        builder.Property(x => x.ReportDate).IsRequired();
        builder.Property(x => x.RecommendationsHtml).HasColumnType("nvarchar(max)");
        builder.Property(x => x.CreatedAtUtc).IsRequired();
        
        builder.HasIndex(x => new { x.Year, x.Month }).IsUnique();
    }
}