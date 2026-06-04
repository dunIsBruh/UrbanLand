using SceneDesign.Application.Queries.GetScene;

namespace SceneDesign.Presentation.Models;

/// <summary>
/// Detailed scene information including objects and layers.
/// </summary>
public sealed record SceneResponse
{
    /// <summary>
    /// Unique identifier of the scene.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Identifier of the project this scene belongs to.
    /// </summary>
    public Guid ProjectId { get; init; }

    /// <summary>
    /// Current view mode (2D or 3D).
    /// </summary>
    /// <example>3D</example>
    public string ViewMode { get; init; } = string.Empty;

    /// <summary>
    /// Version number of the scene.
    /// </summary>
    public int Version { get; init; }

    /// <summary>
    /// Date and time when the scene was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// Whether the scene is locked for editing.
    /// </summary>
    public bool IsLocked { get; init; }

    /// <summary>
    /// List of objects placed in the scene.
    /// </summary>
    public List<SceneObjectResponse> Objects { get; init; } = [];

    /// <summary>
    /// List of layers in the scene.
    /// </summary>
    public List<ObjectLayerResponse> Layers { get; init; } = [];

    public static SceneResponse FromDto(SceneDto dto) => new()
    {
        Id = dto.Id,
        ProjectId = dto.ProjectId,
        ViewMode = dto.ViewMode,
        Version = dto.Version,
        CreatedAt = dto.CreatedAt,
        IsLocked = dto.IsLocked,
        Objects = dto.Objects.Select(SceneObjectResponse.FromDto).ToList(),
        Layers = dto.Layers.Select(l => new ObjectLayerResponse
        {
            Id = l.Id,
            Name = l.Name,
            Order = l.Order,
            IsVisible = l.IsVisible,
            IsLocked = l.IsLocked
        }).ToList()
    };
}

/// <summary>
/// Information about a scene object and its transform.
/// </summary>
public sealed record SceneObjectResponse
{
    /// <summary>
    /// Unique identifier of the scene object.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Identifier of the asset used for this object.
    /// </summary>
    public Guid AssetId { get; init; }

    /// <summary>
    /// X coordinate position in the scene.
    /// </summary>
    /// <example>10.5</example>
    public double PositionX { get; init; }

    /// <summary>
    /// Y coordinate position in the scene.
    /// </summary>
    /// <example>0.0</example>
    public double PositionY { get; init; }

    /// <summary>
    /// Z coordinate position in the scene.
    /// </summary>
    /// <example>5.2</example>
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
    public double ScaleX { get; init; }

    /// <summary>
    /// Y-axis scale factor.
    /// </summary>
    /// <example>1.0</example>
    public double ScaleY { get; init; }

    /// <summary>
    /// Z-axis scale factor.
    /// </summary>
    /// <example>1.0</example>
    public double ScaleZ { get; init; }

    /// <summary>
    /// Identifier of the layer this object belongs to.
    /// </summary>
    public Guid LayerId { get; init; }

    /// <summary>
    /// Minimum X of the bounding box.
    /// </summary>
    public double BoundingBoxMinX { get; init; }

    /// <summary>
    /// Minimum Y of the bounding box.
    /// </summary>
    public double BoundingBoxMinY { get; init; }

    /// <summary>
    /// Minimum Z of the bounding box.
    /// </summary>
    public double BoundingBoxMinZ { get; init; }

    /// <summary>
    /// Maximum X of the bounding box.
    /// </summary>
    public double BoundingBoxMaxX { get; init; }

    /// <summary>
    /// Maximum Y of the bounding box.
    /// </summary>
    public double BoundingBoxMaxY { get; init; }

    /// <summary>
    /// Maximum Z of the bounding box.
    /// </summary>
    public double BoundingBoxMaxZ { get; init; }

    public static SceneObjectResponse FromDto(SceneObjectDto dto) => new()
    {
        Id = dto.Id,
        AssetId = dto.AssetId,
        PositionX = dto.PositionX,
        PositionY = dto.PositionY,
        PositionZ = dto.PositionZ,
        RotationYaw = dto.RotationYaw,
        RotationPitch = dto.RotationPitch,
        RotationRoll = dto.RotationRoll,
        ScaleX = dto.ScaleX,
        ScaleY = dto.ScaleY,
        ScaleZ = dto.ScaleZ,
        LayerId = dto.LayerId,
        BoundingBoxMinX = dto.BoundingBoxMinX,
        BoundingBoxMinY = dto.BoundingBoxMinY,
        BoundingBoxMinZ = dto.BoundingBoxMinZ,
        BoundingBoxMaxX = dto.BoundingBoxMaxX,
        BoundingBoxMaxY = dto.BoundingBoxMaxY,
        BoundingBoxMaxZ = dto.BoundingBoxMaxZ
    };
}

/// <summary>
/// Information about a layer in the scene.
/// </summary>
public sealed record ObjectLayerResponse
{
    /// <summary>
    /// Unique identifier of the layer.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Display name of the layer.
    /// </summary>
    /// <example>Ground</example>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Rendering order of the layer.
    /// </summary>
    public int Order { get; init; }

    /// <summary>
    /// Whether the layer is visible.
    /// </summary>
    public bool IsVisible { get; init; }

    /// <summary>
    /// Whether the layer is locked for editing.
    /// </summary>
    public bool IsLocked { get; init; }
}
