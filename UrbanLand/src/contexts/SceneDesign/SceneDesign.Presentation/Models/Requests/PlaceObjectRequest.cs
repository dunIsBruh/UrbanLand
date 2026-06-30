namespace SceneDesign.Presentation.Models.Requests;

/// <summary>
/// Request model for placing an object in the scene.
/// </summary>
public sealed record PlaceObjectRequest
{
    /// <summary>
    /// Identifier of the asset to place.
    /// </summary>
    [Required]
    public Guid AssetId { get; init; }

    /// <summary>
    /// X coordinate for the object position.
    /// </summary>
    /// <example>10.5</example>
    [Required]
    public double PositionX { get; init; }

    /// <summary>
    /// Y coordinate for the object position.
    /// </summary>
    /// <example>0.0</example>
    [Required]
    public double PositionY { get; init; }

    /// <summary>
    /// Z coordinate for the object position.
    /// </summary>
    /// <example>5.2</example>
    [Required]
    public double PositionZ { get; init; }

    /// <summary>
    /// Yaw rotation in degrees.
    /// </summary>
    public double RotationYaw { get; init; }

    /// <summary>
    /// Pitch rotation in degrees.
    /// </summary>
    public double RotationPitch { get; init; }

    /// <summary>
    /// Roll rotation in degrees.
    /// </summary>
    public double RotationRoll { get; init; }

    /// <summary>
    /// X-axis scale factor.
    /// </summary>
    /// <example>1.0</example>
    public double ScaleX { get; init; } = 1;

    /// <summary>
    /// Y-axis scale factor.
    /// </summary>
    /// <example>1.0</example>
    public double ScaleY { get; init; } = 1;

    /// <summary>
    /// Z-axis scale factor.
    /// </summary>
    /// <example>1.0</example>
    public double ScaleZ { get; init; } = 1;

    /// <summary>
    /// Name of the layer to place the object on. Uses default layer if not specified.
    /// </summary>
    /// <example>Ground</example>
    public string? LayerName { get; init; }
}
