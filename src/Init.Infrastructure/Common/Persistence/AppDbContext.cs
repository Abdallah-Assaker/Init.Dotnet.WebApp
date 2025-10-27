using System.Reflection;
using Init.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Init.Domain.AIRecommendations;

namespace Init.Infrastructure.Common.Persistence;

public class AppDbContext(DbContextOptions options) : DbContext(options), IUnitOfWork
{
    public DbSet<AIRecommendation> AIRecommendations => Set<AIRecommendation>();

    public async Task CommitChangesAsync(CancellationToken cancellationToken = default)
    {
        await SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}