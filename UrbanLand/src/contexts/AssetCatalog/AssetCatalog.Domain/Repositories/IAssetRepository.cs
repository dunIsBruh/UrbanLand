using AssetCatalog.Domain.Entities;
using AssetCatalog.Domain.Enums;
using AssetCatalog.Domain.ValueObjects;
using Core.Abstractions;
using Core.Identity;

namespace AssetCatalog.Domain.Repositories;

public interface IAssetRepository : IRepository<AssetTemplate, AssetId>
{
    Task<IReadOnlyList<AssetTemplate>> GetByCategoryAsync(AssetCategory category, CancellationToken ct = default);
    Task<IReadOnlyList<AssetTemplate>> GetByUploaderIdAsync(UserId uploaderId, CancellationToken ct = default);
    Task<IReadOnlyList<AssetTemplate>> GetTopographicSymbolsAsync(CancellationToken ct = default);
}