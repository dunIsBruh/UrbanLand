using Core.Abstractions;
using Core.Identity;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using SceneDesign.Domain.Entities;
using SceneDesign.Infrastructure.Persistence.Configurations;

namespace SceneDesign.Infrastructure.Persistence;

public class SceneDesignDbContext(
    DbContextOptions<SceneDesignDbContext> options,
    IPublishEndpoint publishEndpoint,
    ICurrentUserAccessor? currentUserService = null)
    : DbContext(options)
{
    public DbSet<Scene> Scenes => Set<Scene>();
    public DbSet<SceneObject> SceneObjects => Set<SceneObject>();
    public DbSet<ObjectLayer> ObjectLayers => Set<ObjectLayer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("scene_design");

        modelBuilder.Ignore<DomainEvent>();

        modelBuilder.ApplyConfiguration(new SceneConfiguration());
        modelBuilder.ApplyConfiguration(new SceneObjectConfiguration());
        modelBuilder.ApplyConfiguration(new ObjectLayerConfiguration());

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditableEntities();
        var result = await base.SaveChangesAsync(cancellationToken);

        await DispatchDomainEventsAsync();

        return result;
    }

    private async Task DispatchDomainEventsAsync()
    {
        var domainEntities = ChangeTracker
            .Entries<IEntity>()
            .Where(x => x.Entity.DomainEvents.Count != 0)
            .Select(x => x.Entity)
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.DomainEvents)
            .ToList();

        domainEntities.ForEach(entity => entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await publishEndpoint.Publish(domainEvent);
        }
    }

    private void UpdateAuditableEntities()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.State is EntityState.Added or EntityState.Modified);

        var now = DateTime.UtcNow;
        var userId = currentUserService?.UserId ?? Guid.Empty;

        foreach (var entry in entries)
        {
            if (entry.Entity is Scene scene)
            {
                if (entry.State == EntityState.Added)
                {
                }
            }
        }
    }
}
