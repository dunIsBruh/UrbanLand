using AssetCatalog.Domain.Repositories;
using MediatR;
using Core.Primitives;

namespace AssetCatalog.Application.Queries.GetTopographicSymbols;

public class GetTopographicSymbolsQueryHandler(
    IAssetRepository assetRepository)
    : IRequestHandler<GetTopographicSymbolsQuery, Result<List<TopographicSymbolDto>>>
{
    public async Task<Result<List<TopographicSymbolDto>>> Handle(GetTopographicSymbolsQuery query, CancellationToken ct)
    {
        var assets = await assetRepository.GetTopographicSymbolsAsync(ct);
        var dtos = assets
            .Where(a => a.TopographicSymbol != null)
            .Select(a => new TopographicSymbolDto(
                a.TopographicSymbol!.SymbolType,
                a.TopographicSymbol.Color,
                a.TopographicSymbol.Size))
            .ToList();

        return Result<List<TopographicSymbolDto>>.Success(dtos);
    }
}
