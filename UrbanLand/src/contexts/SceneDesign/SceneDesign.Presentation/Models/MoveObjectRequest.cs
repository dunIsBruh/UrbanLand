using System.ComponentModel.DataAnnotations;

namespace SceneDesign.Presentation.Models;

/// <summary>
/// Request model for moving an object to a new position.
/// </summary>
public sealed record MoveObjectRequest
{
    /// <summary>
    /// New X coordinate for the object.
    /// </summary>
    /// <example>15.0</example>
    [Required]
    public double NewPositionX { get; init; }

    /// <summary>
    /// New Y coordinate for the object.
    /// </summary>
    /// <example>2.5</example>
    [Required]
    public double NewPositionY { get; init; }

    /// <summary>
    /// New Z coordinate for the object.
    /// </summary>
    /// <example>8.0</example>
    [Required]
    public double NewPositionZ { get; init; }
}
