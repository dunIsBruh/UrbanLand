using System.ComponentModel.DataAnnotations;

namespace AssetCatalog.Presentation.Models;

/// <summary>
/// Request model for importing a model from Sketchfab.
/// </summary>
public sealed record ImportFromSketchfabRequest
{
    /// <summary>
    /// Sketchfab model ID to import.
    /// </summary>
    /// <example>abc123def</example>
    [Required]
    public string SketchfabModelId { get; init; } = string.Empty;

    /// <summary>
    /// Display name for the imported asset.
    /// </summary>
    /// <example>Oak Tree</example>
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Optional description for the imported asset.
    /// </summary>
    /// <example>Imported from Sketchfab</example>
    [StringLength(2000)]
    public string? Description { get; init; }
}
