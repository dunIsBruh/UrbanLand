using System.ComponentModel.DataAnnotations;

namespace SceneDesign.Presentation.Models;

public sealed record MoveObjectRequest
{
    [Required]
    public double NewPositionX { get; init; }
    [Required]
    public double NewPositionY { get; init; }
    [Required]
    public double NewPositionZ { get; init; }
}
