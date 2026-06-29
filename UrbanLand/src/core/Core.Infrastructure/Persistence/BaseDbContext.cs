using System.Reflection;
using Core.Abstractions;
using Core.Identity;
using MassTransit.Mediator;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Persistence;

public abstract class BaseDbContext : DbContext
{
    private readonly IMediator _mediator;
    // private readonly ICurrentUserService? _currentUserService;

    protected BaseDbContext(
        DbContextOptions options,
        IMediator mediator,
        ICurrentUserAccessor? currentUserService = null)
        : base(options)
    {
        _mediator = mediator;
        // _currentUserService = currentUserService;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await DispatchDomainEventsAsync();

        // UpdateAuditableEntities();

        return await base.SaveChangesAsync(cancellationToken);
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
            await _mediator.Publish(domainEvent);
        }
    }
}
