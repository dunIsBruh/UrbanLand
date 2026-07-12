using Core.Primitives;
using MediatR;
using SceneDesign.Domain.Entities;

namespace SceneDesign.Application.Commands.PlaceObject;

public record PlaceObjectCommand(
    Guid SceneId,
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
    string? LayerName = null) : IRequest<Result<SceneObject>>;
