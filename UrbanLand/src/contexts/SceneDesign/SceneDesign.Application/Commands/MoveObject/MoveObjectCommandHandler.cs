using MediatR;
using SceneDesign.Domain.Entities;
using SceneDesign.Domain.Repositories;
using SceneDesign.Domain.ValueObjects;
using SharedKernel.Primitives;

namespace SceneDesign.Application.Commands.MoveObject;

public class MoveObjectCommandHandler(
    ISceneRepository sceneRepository)
    : IRequestHandler<MoveObjectCommand, Result>
{
    public async Task<Result> Handle(MoveObjectCommand command, CancellationToken ct)
    {
        var sceneId = SceneId.From(command.SceneId);
        var scene = await sceneRepository.GetByIdAsync(sceneId, ct);
        if (scene == null)
            return Result.Failure(Error.NotFound(nameof(Scene), sceneId));

        var objectId = SceneObjectId.From(command.ObjectId);
        var newPosition = new Position3D(command.NewPositionX, command.NewPositionY, command.NewPositionZ);

        var result = scene.MoveObject(objectId, newPosition);
        if (result.IsFailure)
            return result;

        await sceneRepository.SaveAsync(scene, ct);
        return Result.Success();
    }
}
