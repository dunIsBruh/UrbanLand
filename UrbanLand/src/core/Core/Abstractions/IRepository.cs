namespace Core.Abstractions;

public interface IRepository<TEntity, in TId> 
    where TEntity : Entity<TId> 
    where TId : notnull
{
    Task<TEntity?> GetByIdAsync(TId id, CancellationToken ct = default);
    Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken ct = default);
    Task SaveAsync(TEntity entity, CancellationToken ct = default);
    Task DeleteAsync(TEntity entity, CancellationToken ct = default);
}
