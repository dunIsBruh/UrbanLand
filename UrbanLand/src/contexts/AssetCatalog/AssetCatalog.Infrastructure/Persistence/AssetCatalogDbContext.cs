using AssetCatalog.Domain.Entities;
using AssetCatalog.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Abstractions;

namespace AssetCatalog.Infrastructure.Persistence;

public class AssetCatalogDbContext(
    DbContextOptions<AssetCatalogDbContext> options)
    : DbContext(options)
{
    public DbSet<AssetTemplate> Assets => Set<AssetTemplate>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("asset_catalog");

        modelBuilder.Ignore<DomainEvent>();

        modelBuilder.ApplyConfiguration(new AssetTemplateConfiguration());
        modelBuilder.ApplyConfiguration(new AssetVersionConfiguration());

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}
