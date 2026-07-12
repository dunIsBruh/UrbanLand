using AssetCatalog.Domain.Enums;
using AssetCatalog.Domain.Repositories;
using MediatR;
using Core.Primitives;

namespace AssetCatalog.Application.Queries.GetAssetsByCategory;

public class GetAssetsByCategoryQueryHandler(
    IAssetRepository assetRepository)
    : IRequestHandler<GetAssetsByCategoryQuery, Result<List<AssetSummaryDto>>>
{
    public async Task<Result<List<AssetSummaryDto>>> Handle(GetAssetsByCategoryQuery query, CancellationToken ct)
    {
        var assets = await assetRepository.GetByCategoryAsync(query.Category, ct);
        var dtos = assets.Select(AssetSummaryDto.FromDomain).ToList();
        return Result<List<AssetSummaryDto>>.Success(dtos);
    }
}
