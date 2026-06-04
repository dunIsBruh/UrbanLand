using System.ComponentModel.DataAnnotations;

namespace SceneDesign.Presentation.Models;

public sealed record PlaceObjectRequest
{
    [Required]
    public Guid AssetId { get; init; }

    [Required]
    public double PositionX { get; init; }
    [Required]
    public double PositionY { get; init; }
    [Required]
    public double PositionZ { get; init; }

    public double RotationYaw { get; init; }
    public double RotationPitch { get; init; }
    public double RotationRoll { get; init; }

    public double ScaleX { get; init; } = 1;
    public double ScaleY { get; init; } = 1;
    public double ScaleZ { get; init; } = 1;

    public string? LayerName { get; init; }
}
