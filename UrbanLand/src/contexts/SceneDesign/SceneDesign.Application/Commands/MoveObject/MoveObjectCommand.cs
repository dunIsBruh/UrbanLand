using MediatR;
using SharedKernel.Primitives;

namespace SceneDesign.Application.Commands.MoveObject;

public record MoveObjectCommand(
    Guid SceneId,
    Guid ObjectId,
    double NewPositionX,
    double NewPositionY,
    double NewPositionZ) : IRequest<Result>;
