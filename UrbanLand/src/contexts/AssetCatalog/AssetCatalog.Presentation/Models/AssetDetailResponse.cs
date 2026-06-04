using AssetCatalog.Application.Queries.GetAsset;

namespace AssetCatalog.Presentation.Models;

public sealed record AssetDetailResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public List<AssetVersionDetail> Versions { get; init; } = [];
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

public sealed record AssetVersionDetail
{
    public int VersionNumber { get; init; }
    public string FileUrl { get; init; } = string.Empty;
    public string FileName { get; init; } = string.Empty;
    public string Format { get; init; } = string.Empty;
    public double Width { get; init; }
    public double Height { get; init; }
    public double Depth { get; init; }
    public int PolygonCount { get; init; }
    public DateTime CreatedAt { get; init; }
}

public sealed record TopographicSymbolDetail
{
    public string SymbolType { get; init; } = string.Empty;
    public string Color { get; init; } = string.Empty;
    public double Size { get; init; }
}
