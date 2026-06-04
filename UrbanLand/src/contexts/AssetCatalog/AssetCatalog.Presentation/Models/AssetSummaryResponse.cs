using AssetCatalog.Application.Queries.GetAssetsByCategory;

namespace AssetCatalog.Presentation.Models;

public sealed record AssetSummaryResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public double Width { get; init; }
    public double Height { get; init; }
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
