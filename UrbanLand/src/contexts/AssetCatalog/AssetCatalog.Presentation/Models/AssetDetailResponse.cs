using AssetCatalog.Application.Queries.GetAsset;

namespace AssetCatalog.Presentation.Models;

/// <summary>
/// Detailed information about an asset including versions and topographic symbol data.
/// </summary>
public sealed record AssetDetailResponse
{
    /// <summary>
    /// Unique identifier of the asset.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Display name of the asset.
    /// </summary>
    /// <example>Oak Tree</example>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Detailed description of the asset.
    /// </summary>
    /// <example>A high-polygon oak tree model for urban landscaping</example>
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// Category of the asset.
    /// </summary>
    /// <example>Vegetation</example>
    public string Category { get; init; } = string.Empty;

    /// <summary>
    /// Approval status of the asset.
    /// </summary>
    /// <example>Approved</example>
    public string Status { get; init; } = string.Empty;

    /// <summary>
    /// Date and time when the asset was uploaded.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// List of versions available for this asset.
    /// </summary>
    public List<AssetVersionDetail> Versions { get; init; } = [];

    /// <summary>
    /// Topographic symbol data for 2D map mode, if applicable.
    /// </summary>
    public TopographicSymbolDetail? TopographicSymbol { get; init; }

    public static AssetDetailResponse FromDto(AssetDto dto) => new()
    {
        Id = dto.Id,
        Name = dto.Name,
        Description = dto.Description,
        Category = dto.Category,
        Status = dto.Status,
        CreatedAt = dto.CreatedAt,
        Versions = dto.Versions.Select(v => new AssetVersionDetail
        {
            VersionNumber = v.VersionNumber,
            FileUrl = v.FileUrl,
            FileName = v.FileName,
            Format = v.Format,
            Width = v.Width,
            Height = v.Height,
            Depth = v.Depth,
            PolygonCount = v.PolygonCount,
            CreatedAt = v.CreatedAt
        }).ToList(),
        TopographicSymbol = dto.TopographicSymbol != null
            ? new TopographicSymbolDetail
            {
                SymbolType = dto.TopographicSymbol.SymbolType,
                Color = dto.TopographicSymbol.Color,
                Size = dto.TopographicSymbol.Size
            }
            : null
    };
}

/// <summary>
/// Information about a specific version of an asset.
/// </summary>
public sealed record AssetVersionDetail
{
    /// <summary>
    /// Version number of the asset.
    /// </summary>
    public int VersionNumber { get; init; }

    /// <summary>
    /// URL to download the asset file.
    /// </summary>
    /// <example>https://assets.example.com/models/oak-v2.glb</example>
    public string FileUrl { get; init; } = string.Empty;

    /// <summary>
    /// Original file name of the asset.
    /// </summary>
    /// <example>oak-v2.glb</example>
    public string FileName { get; init; } = string.Empty;

    /// <summary>
    /// File format of the asset (glb, fbx, obj, etc.).
    /// </summary>
    /// <example>glb</example>
    public string Format { get; init; } = string.Empty;

    /// <summary>
    /// Width of the asset in world units.
    /// </summary>
    /// <example>2.5</example>
    public double Width { get; init; }

    /// <summary>
    /// Height of the asset in world units.
    /// </summary>
    /// <example>5.0</example>
    public double Height { get; init; }

    /// <summary>
    /// Depth of the asset in world units.
    /// </summary>
    /// <example>2.5</example>
    public double Depth { get; init; }

    /// <summary>
    /// Number of polygons in the 3D model.
    /// </summary>
    /// <example>15000</example>
    public int PolygonCount { get; init; }

    /// <summary>
    /// Date and time when this version was uploaded.
    /// </summary>
    public DateTime CreatedAt { get; init; }
}

/// <summary>
/// Topographic symbol data used for 2D map rendering.
/// </summary>
public sealed record TopographicSymbolDetail
{
    /// <summary>
    /// Type of the topographic symbol.
    /// </summary>
    /// <example>Tree</example>
    public string SymbolType { get; init; } = string.Empty;

    /// <summary>
    /// Color of the symbol in hex format.
    /// </summary>
    /// <example>#00FF00</example>
    public string Color { get; init; } = string.Empty;

    /// <summary>
    /// Size of the symbol in map units.
    /// </summary>
    /// <example>1.5</example>
    public double Size { get; init; }
}
