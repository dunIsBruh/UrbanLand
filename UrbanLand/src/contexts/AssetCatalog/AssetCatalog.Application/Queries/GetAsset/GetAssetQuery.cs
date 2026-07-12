using MediatR;
using Core.Primitives;

namespace AssetCatalog.Application.Queries.GetAsset;

public record GetAssetQuery(Guid AssetId) : IRequest<Result<AssetDto>>;
