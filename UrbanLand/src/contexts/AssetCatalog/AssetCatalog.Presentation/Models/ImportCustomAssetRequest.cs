using System.ComponentModel.DataAnnotations;

namespace AssetCatalog.Presentation.Models;

public sealed record ImportCustomAssetRequest
{
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Name { get; init; } = string.Empty;

    [StringLength(2000)]
    public string? Description { get; init; }

    [Required]
    public string FileUrl { get; init; } = string.Empty;

    [Required]
    public string FileName { get; init; } = string.Empty;

    [Required]
    [Range(1, 100 * 1024 * 1024)]
    public long FileSize { get; init; }

    [Required]
    public string Format { get; init; } = string.Empty;

    [Required]
    [Range(0.001, double.MaxValue)]
    public double Width { get; init; } = 1;

    [Required]
    [Range(0.001, double.MaxValue)]
    public double Height { get; init; } = 1;

    [Required]
    [Range(0.001, double.MaxValue)]
    public double Depth { get; init; } = 1;

    public int PolygonCount { get; init; }
}
