using SceneDesign.Application.Queries.GetScene;

namespace SceneDesign.Presentation.Models;

public sealed record SceneResponse
{
    public Guid Id { get; init; }
    public Guid ProjectId { get; init; }
    public string ViewMode { get; init; } = string.Empty;
    public int Version { get; init; }
    public DateTime CreatedAt { get; init; }
    public bool IsLocked { get; init; }
    public List<SceneObjectResponse> Objects { get; init; } = [];
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

public sealed record SceneObjectResponse
{
    public Guid Id { get; init; }
    public Guid AssetId { get; init; }
    public double PositionX { get; init; }
    public double PositionY { get; init; }
    public double PositionZ { get; init; }
    public double RotationYaw { get; init; }
    public double RotationPitch { get; init; }
    public double RotationRoll { get; init; }
    public double ScaleX { get; init; }
    public double ScaleY { get; init; }
    public double ScaleZ { get; init; }
    public Guid LayerId { get; init; }
    public double BoundingBoxMinX { get; init; }
    public double BoundingBoxMinY { get; init; }
    public double BoundingBoxMinZ { get; init; }
    public double BoundingBoxMaxX { get; init; }
    public double BoundingBoxMaxY { get; init; }
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

public sealed record ObjectLayerResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int Order { get; init; }
    public bool IsVisible { get; init; }
    public bool IsLocked { get; init; }
}
