using AssetCatalog.Domain.Entities;

namespace AssetCatalog.Application.Queries.GetAssetsByCategory;

public record AssetSummaryDto(
    Guid Id,
    string Name,
    string Description,
    string Category,
    string Status,
    DateTime CreatedAt,
    double Width,
    double Height,
    double Depth)
{
    public static AssetSummaryDto FromDomain(AssetTemplate asset)
    {
        var current = asset.CurrentVersion;
        return new AssetSummaryDto(
            asset.Id.Value,
            asset.Name,
            asset.Description,
            asset.Category.ToString(),
            asset.Status.ToString(),
            asset.CreatedAt,
            current.Model.Dimensions.Width,
            current.Model.Dimensions.Height,
            current.Model.Dimensions.Depth);
    }
}
