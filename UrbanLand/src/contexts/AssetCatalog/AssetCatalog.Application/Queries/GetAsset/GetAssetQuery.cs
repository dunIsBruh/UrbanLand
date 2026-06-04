using MediatR;
using SharedKernel.Primitives;

namespace AssetCatalog.Application.Queries.GetAsset;

public record GetAssetQuery(Guid AssetId) : IRequest<Result<AssetDto>>;
