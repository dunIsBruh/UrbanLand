using Microsoft.EntityFrameworkCore;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Repositories;
using ProjectManagement.Domain.ValueObjects;
using Core.Identity;
using ProjectId = Core.Contracts.ProjectId;

namespace ProjectManagement.Infrastructure.Persistence.Repositories;

public class ProjectRepository(ProjectManagementDbContext context) : IProjectRepository
{
    public async Task<Project?> GetByIdAsync(ProjectId id, CancellationToken ct = default)
    {
        return await context.Projects
            .Include(p => p.Members)
            .Include(p => p.Invitations)
            .FirstOrDefaultAsync(p => p.Id == id, 
            ct);
    }

    public async Task<IReadOnlyList<Project>> GetAllAsync(CancellationToken ct = default)
    {
        return await context.Projects
            .Include(p => p.Members)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task SaveAsync(Project project, CancellationToken ct = default)
    {
        var entry = context.Entry(project);
        
        if (entry.State == EntityState.Detached)
        {
            var exists = await ExistsAsync(project.Id, ct);
            
            if (exists)
            {
                context.Projects.Update(project);
            }
            else
            {
                context.Projects.Add(project);
            }
        }
        
        await context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Project project, CancellationToken ct = default)
    {
        context.Projects.Remove(project);
        await context.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<Project>> GetByUserIdAsync(
        UserId userId, 
        CancellationToken ct = default)
    {
        return await context.Projects
            .Include(p => p.Members)
            .Where(p => p.OwnerId == userId || p.Members.Any(m => m.MemberId == userId))
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<Project>> GetByObserverIdAsync(
        UserId observerId, 
        CancellationToken ct = default)
    {
        return await context.Projects
            .Include(p => p.Members)
            .Where(p => p.Members.Any(m => 
                m.MemberId == observerId && 
                m.Role == ProjectRole.Visitor))
            .AsNoTracking()
            .ToListAsync(ct);
    }

    private async Task<bool> ExistsAsync(ProjectId id, CancellationToken ct = default)
    {
        return await context.Projects.AnyAsync(p => p.Id == id, ct);
    }
}