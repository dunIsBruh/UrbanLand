using AssetCatalog.Domain.Entities;

namespace AssetCatalog.Application.Queries.GetAsset;

public record AssetDto(
    Guid Id,
    string Name,
    string Description,
    string Category,
    bool IsCustom,
    Guid UploaderId,
    string Status,
    DateTime CreatedAt,
    List<AssetVersionDto> Versions,
    List<string> Tags,
    TopographicSymbolDto? TopographicSymbol)
{
    public static AssetDto FromDomain(AssetTemplate asset) => new(
        asset.Id.Value,
        asset.Name,
        asset.Description,
        asset.Category.ToString(),
        asset.IsCustom,
        asset.UploaderId.Value,
        asset.Status.ToString(),
        asset.CreatedAt,
        asset.Versions.Select(AssetVersionDto.FromDomain).ToList(),
        asset.Tags.ToList(),
        asset.TopographicSymbol != null
            ? new TopographicSymbolDto(
                asset.TopographicSymbol.SymbolType,
                asset.TopographicSymbol.Color,
                asset.TopographicSymbol.Size)
            : null);
}

public record AssetVersionDto(
    int VersionNumber,
    string FileUrl,
    string FileName,
    long FileSize,
    string Format,
    double Width,
    double Height,
    double Depth,
    int PolygonCount,
    DateTime CreatedAt)
{
    public static AssetVersionDto FromDomain(AssetVersion version) => new(
        version.VersionNumber,
        version.Model.FileUrl,
        version.Model.FileName,
        version.Model.FileSize,
        version.Model.Format,
        version.Model.Dimensions.Width,
        version.Model.Dimensions.Height,
        version.Model.Dimensions.Depth,
        version.Model.PolygonCount,
        version.CreatedAt);
}

public record TopographicSymbolDto(string SymbolType, string Color, double Size);
