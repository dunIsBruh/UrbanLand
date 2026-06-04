namespace SharedKernel.Abstractions;

public abstract class Entity<TId> : IEquatable<Entity<TId>>, IEntity where TId : notnull
{
    private readonly List<DomainEvent> _domainEvents = [];
    
    public TId Id { get; } = default!;
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    
    protected Entity() {}
    
    protected Entity(TId id)
    {
        Id = id;
    }
    
    public override bool Equals(object? obj) => Equals(obj as Entity<TId>);
    
    public bool Equals(Entity<TId>? other)
    {
        if (other is null)
        {
            return false;
        }
        
        return ReferenceEquals(this, other) || EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }    
    
    public override int GetHashCode()
        => HashCode.Combine(GetType(), Id);

    public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
        => Equals(left, right);

    public static bool operator !=(Entity<TId>? left, Entity<TId>? right)
        => !Equals(left, right);
    
    protected void AddDomainEvent(DomainEvent domainEvent)
        => _domainEvents.Add(domainEvent);
    

    public IReadOnlyCollection<DomainEvent> ClearDomainEvents()
    {
        var events = _domainEvents.ToArray();
        _domainEvents.Clear();
        return events;
    }
}
