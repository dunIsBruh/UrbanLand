using AssetCatalog.Application.Queries.GetAssetsByCategory;

namespace AssetCatalog.Presentation.Models.Responses;

/// <summary>
/// Summary information about an asset in the catalog.
/// </summary>
public sealed record AssetSummaryResponse
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
    /// Category of the asset (Vegetation, Buildings, etc.).
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

    public static AssetSummaryResponse FromDto(AssetSummaryDto dto) => new()
    {
        Id = dto.Id,
        Name = dto.Name,
        Category = dto.Category,
        Status = dto.Status,
        CreatedAt = dto.CreatedAt,
        Width = dto.Width,
        Height = dto.Height,
        Depth = dto.Depth
    };
}
