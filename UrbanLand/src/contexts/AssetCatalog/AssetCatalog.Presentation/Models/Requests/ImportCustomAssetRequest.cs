using System.ComponentModel.DataAnnotations;

namespace AssetCatalog.Presentation.Models.Requests;

/// <summary>
/// Request model for importing a custom 3D asset.
/// </summary>
public sealed record ImportCustomAssetRequest
{
    /// <summary>
    /// Name of the asset.
    /// </summary>
    /// <example>Oak Tree</example>
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Optional description of the asset.
    /// </summary>
    /// <example>A high-polygon oak tree model</example>
    [StringLength(2000)]
    public string? Description { get; init; }

    /// <summary>
    /// URL where the asset file is hosted.
    /// </summary>
    /// <example>https://storage.example.com/models/oak.glb</example>
    [Required]
    public string FileUrl { get; init; } = string.Empty;

    /// <summary>
    /// Original file name with extension.
    /// </summary>
    /// <example>oak.glb</example>
    [Required]
    public string FileName { get; init; } = string.Empty;

    /// <summary>
    /// File size in bytes (max 100 MB).
    /// </summary>
    /// <example>5242880</example>
    [Required]
    [Range(1, 100 * 1024 * 1024)]
    public long FileSize { get; init; }

    /// <summary>
    /// File format (glb, fbx, obj, etc.).
    /// </summary>
    /// <example>glb</example>
    [Required]
    public string Format { get; init; } = string.Empty;

    /// <summary>
    /// Width of the asset in world units.
    /// </summary>
    /// <example>2.5</example>
    [Required]
    [Range(0.001, double.MaxValue)]
    public double Width { get; init; } = 1;

    /// <summary>
    /// Height of the asset in world units.
    /// </summary>
    /// <example>5.0</example>
    [Required]
    [Range(0.001, double.MaxValue)]
    public double Height { get; init; } = 1;

    /// <summary>
    /// Depth of the asset in world units.
    /// </summary>
    /// <example>2.5</example>
    [Required]
    [Range(0.001, double.MaxValue)]
    public double Depth { get; init; } = 1;

    /// <summary>
    /// Number of polygons in the 3D model.
    /// </summary>
    /// <example>15000</example>
    public int PolygonCount { get; init; }
}
