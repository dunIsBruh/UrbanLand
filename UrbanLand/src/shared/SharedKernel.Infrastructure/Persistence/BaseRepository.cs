// using SharedKernel.Abstractions;
//
// namespace SharedKernel.Infrastructure;
//
// public abstract class BaseRepository<T> : IRepository<T> where T : AggregateRoot<Guid>
// {
//     protected readonly DbContext Context;
//     protected readonly DbSet<T> DbSet;
//
//     protected BaseRepository(DbContext context)
//     {
//         Context = context;
//         DbSet = context.Set<T>();
//     }
//
//     public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
//     {
//         return await DbSet.FindAsync(new object[] { id }, ct);
//     }
//
//     public virtual async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default)
//     {
//         return await DbSet.ToListAsync(ct);
//     }
//
//     public Task<T> SaveAsync(T entity, CancellationToken ct = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task DeleteAsync(T entity, CancellationToken ct = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public virtual async Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
//     {
//         return await DbSet.AnyAsync(e => e.Id == id, ct);
//     }
// }





// public class ProjectRepository : IRepository<Project>
// {
//     private readonly ProjectManagementDbContext _context;
//
//     public ProjectRepository(ProjectManagementDbContext context)
//     {
//         _context = context;
//     }
//
//     public async Task<Project?> GetByIdAsync(Guid id, CancellationToken ct = default)
//     {
//         return await _context.Projects
//             .Include(p => p.AccessList) // Загружаем связанные данные
//             .FirstOrDefaultAsync(p => p.Id == id, ct);
//     }
//
//     public async Task<IReadOnlyList<Project>> GetAllAsync(CancellationToken ct = default)
//     {
//         return await _context.Projects
//             .Include(p => p.AccessList)
//             .ToListAsync(ct);
//     }
//
//     // Единый метод сохранения
//     public async Task SaveAsync(Project project, CancellationToken ct = default)
//     {
//         // EF Core сам определит: insert или update
//         if (_context.Entry(project).State == EntityState.Detached)
//         {
//             _context.Projects.Add(project);
//         }
//         else
//         {
//             _context.Projects.Update(project);
//         }
//
//         await _context.SaveChangesAsync(ct);
//         
//         // Диспатчим доменные события ПОСЛЕ сохранения
//         await DispatchDomainEventsAsync(project);
//     }
//
//     public async Task DeleteAsync(Project project, CancellationToken ct = default)
//     {
//         _context.Projects.Remove(project);
//         await _context.SaveChangesAsync(ct);
//     }
//
//     private async Task DispatchDomainEventsAsync(Project project)
//     {
//         var events = project.DomainEvents.ToList();
//         project.ClearDomainEvents();
//
//         foreach (var domainEvent in events)
//         {
//             // Публикуем через MassTransit/MediatR
//             await PublishDomainEventAsync(domainEvent);
//         }
//     }
// }