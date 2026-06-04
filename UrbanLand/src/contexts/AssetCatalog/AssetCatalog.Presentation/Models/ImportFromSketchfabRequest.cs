using System.ComponentModel.DataAnnotations;

namespace AssetCatalog.Presentation.Models;

public sealed record ImportFromSketchfabRequest
{
    [Required]
    public string SketchfabModelId { get; init; } = string.Empty;

    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Name { get; init; } = string.Empty;

    [StringLength(2000)]
    public string? Description { get; init; }
}
