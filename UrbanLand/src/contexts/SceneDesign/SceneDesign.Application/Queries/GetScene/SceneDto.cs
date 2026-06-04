using SceneDesign.Domain.Entities;
using SceneDesign.Domain.ValueObjects;

namespace SceneDesign.Application.Queries.GetScene;

public record SceneDto(
    Guid Id,
    Guid ProjectId,
    string ViewMode,
    int Version,
    DateTime CreatedAt,
    bool IsLocked,
    List<SceneObjectDto> Objects,
    List<ObjectLayerDto> Layers)
{
    public static SceneDto FromDomain(Scene scene) => new(
        scene.Id.Value,
        scene.ProjectId.Value,
        scene.CurrentViewMode.ToString(),
        scene.Version,
        scene.CreatedAt,
        scene.IsLocked,
        scene.Objects.Select(SceneObjectDto.FromDomain).ToList(),
        scene.Layers.Select(l => new ObjectLayerDto(l.Id.Value, l.Name, l.Order, l.IsVisible, l.IsLocked)).ToList());
}

public record SceneObjectDto(
    Guid Id,
    Guid AssetId,
    double PositionX,
    double PositionY,
    double PositionZ,
    double RotationYaw,
    double RotationPitch,
    double RotationRoll,
    double ScaleX,
    double ScaleY,
    double ScaleZ,
    Guid LayerId,
    double BoundingBoxMinX,
    double BoundingBoxMinY,
    double BoundingBoxMinZ,
    double BoundingBoxMaxX,
    double BoundingBoxMaxY,
    double BoundingBoxMaxZ)
{
    public static SceneObjectDto FromDomain(SceneObject obj) => new(
        obj.Id.Value,
        obj.AssetId.Value,
        obj.Position.X,
        obj.Position.Y,
        obj.Position.Z,
        obj.Rotation.Yaw,
        obj.Rotation.Pitch,
        obj.Rotation.Roll,
        obj.Scale.X,
        obj.Scale.Y,
        obj.Scale.Z,
        obj.LayerId.Value,
        obj.BoundingBox.Min.X,
        obj.BoundingBox.Min.Y,
        obj.BoundingBox.Min.Z,
        obj.BoundingBox.Max.X,
        obj.BoundingBox.Max.Y,
        obj.BoundingBox.Max.Z);
}

public record ObjectLayerDto(
    Guid Id,
    string Name,
    int Order,
    bool IsVisible,
    bool IsLocked);
