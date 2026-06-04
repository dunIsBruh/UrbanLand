namespace SharedKernel.Abstractions;

public interface IEntity
{
    IReadOnlyCollection<DomainEvent> DomainEvents { get; }
    IReadOnlyCollection<DomainEvent> ClearDomainEvents();
}