using AssetCatalog.Domain.Entities;
using AssetCatalog.Domain.Repositories;
using AssetCatalog.Domain.ValueObjects;
using MediatR;
using Core.Primitives;

namespace AssetCatalog.Application.Queries.GetAsset;

public class GetAssetQueryHandler(
    IAssetRepository assetRepository)
    : IRequestHandler<GetAssetQuery, Result<AssetDto>>
{
    public async Task<Result<AssetDto>> Handle(GetAssetQuery query, CancellationToken ct)
    {
        var assetId = AssetId.From(query.AssetId);
        var asset = await assetRepository.GetByIdAsync(assetId, ct);

        if (asset == null)
            return Result<AssetDto>.Failure(Error.NotFound(nameof(AssetTemplate), assetId));

        return Result<AssetDto>.Success(AssetDto.FromDomain(asset));
    }
}
