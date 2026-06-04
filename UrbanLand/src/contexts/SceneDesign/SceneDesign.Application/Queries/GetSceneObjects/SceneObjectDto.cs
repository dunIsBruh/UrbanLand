using SceneDesign.Domain.Entities;

namespace SceneDesign.Application.Queries.GetSceneObjects;

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
        obj.BoundingBox.Max.Z
        );
}
