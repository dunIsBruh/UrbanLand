using AssetCatalog.Domain.Enums;
using MediatR;
using SharedKernel.Primitives;

namespace AssetCatalog.Application.Queries.GetAssetsByCategory;

public record GetAssetsByCategoryQuery(AssetCategory Category) : IRequest<Result<List<AssetSummaryDto>>>;
