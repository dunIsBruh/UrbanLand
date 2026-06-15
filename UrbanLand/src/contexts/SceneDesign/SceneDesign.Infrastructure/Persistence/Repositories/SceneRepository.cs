using Microsoft.EntityFrameworkCore;
using SceneDesign.Domain.Entities;
using SceneDesign.Domain.Repositories;
using SceneDesign.Domain.ValueObjects;
using ProjectId = SharedKernel.Contracts.ProjectId;

namespace SceneDesign.Infrastructure.Persistence.Repositories;

public class SceneRepository(SceneDesignDbContext context) : ISceneRepository
{
    public async Task<Scene?> GetByIdAsync(SceneId id, CancellationToken ct = default)
    {
        return await context.Scenes
            .Include(s => s.Objects)
            .Include(s => s.Layers)
            .FirstOrDefaultAsync(s => s.Id == id, ct);
    }

    public async Task<IReadOnlyList<Scene>> GetAllAsync(CancellationToken ct = default)
    {
        return await context.Scenes
            .Include(s => s.Objects)
            .Include(s => s.Layers)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task SaveAsync(Scene scene, CancellationToken ct = default)
    {
        var entry = context.Entry(scene);
        if (entry.State == EntityState.Detached)
        {
            context.Scenes.Add(scene);
        }

        await context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Scene scene, CancellationToken ct = default)
    {
        context.Scenes.Remove(scene);
        await context.SaveChangesAsync(ct);
    }

    public async Task<Scene?> GetByProjectIdAsync(ProjectId projectId, CancellationToken ct = default)
    {
        return await context.Scenes
            .Include(s => s.Objects)
            .Include(s => s.Layers)
            .FirstOrDefaultAsync(s => s.ProjectId == projectId, ct);
    }

    private async Task<bool> ExistsAsync(SceneId id, CancellationToken ct = default)
    {
        return await context.Scenes.AnyAsync(s => s.Id == id, ct);
    }
}
