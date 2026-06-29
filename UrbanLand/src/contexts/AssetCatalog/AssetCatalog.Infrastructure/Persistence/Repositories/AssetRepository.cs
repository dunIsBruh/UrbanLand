using AssetCatalog.Domain.Entities;
using AssetCatalog.Domain.Enums;
using AssetCatalog.Domain.Repositories;
using AssetCatalog.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Core.Identity;

namespace AssetCatalog.Infrastructure.Persistence.Repositories;

public class AssetRepository(AssetCatalogDbContext context) : IAssetRepository
{
    public async Task<AssetTemplate?> GetByIdAsync(AssetId id, CancellationToken ct = default)
    {
        return await context.Assets
            .Include(a => a.Versions)
            .FirstOrDefaultAsync(a => a.Id == id, ct);
    }

    public async Task<IReadOnlyList<AssetTemplate>> GetAllAsync(CancellationToken ct = default)
    {
        return await context.Assets
            .Include(a => a.Versions)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task SaveAsync(AssetTemplate asset, CancellationToken ct = default)
    {
        var entry = context.Entry(asset);

        if (entry.State == EntityState.Detached)
        {
            var exists = await ExistsAsync(asset.Id, ct);

            if (exists)
            {
                context.Assets.Update(asset);
            }
            else
            {
                context.Assets.Add(asset);
            }
        }

        await context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(AssetTemplate asset, CancellationToken ct = default)
    {
        context.Assets.Remove(asset);
        await context.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<AssetTemplate>> GetByCategoryAsync(AssetCategory category, CancellationToken ct = default)
    {
        return await context.Assets
            .Include(a => a.Versions)
            .Where(a => a.Category == category)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<AssetTemplate>> GetByUploaderIdAsync(UserId uploaderId, CancellationToken ct = default)
    {
        return await context.Assets
            .Include(a => a.Versions)
            .Where(a => a.UploaderId == uploaderId)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<AssetTemplate>> GetTopographicSymbolsAsync(CancellationToken ct = default)
    {
        return await context.Assets
            .Include(a => a.Versions)
            .Where(a => a.TopographicSymbol != null)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    private async Task<bool> ExistsAsync(AssetId id, CancellationToken ct = default)
    {
        return await context.Assets.AnyAsync(a => a.Id == id, ct);
    }
}
