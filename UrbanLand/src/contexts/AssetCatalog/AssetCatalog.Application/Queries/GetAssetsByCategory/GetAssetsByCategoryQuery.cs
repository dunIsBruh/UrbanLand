using AssetCatalog.Domain.Enums;
using MediatR;
using Core.Primitives;

namespace AssetCatalog.Application.Queries.GetAssetsByCategory;

public record GetAssetsByCategoryQuery(AssetCategory Category) : IRequest<Result<List<AssetSummaryDto>>>;
