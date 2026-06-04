using MediatR;
using SceneDesign.Domain.Entities;
using SceneDesign.Domain.Repositories;
using SceneDesign.Domain.ValueObjects;
using SharedKernel.Primitives;

namespace SceneDesign.Application.Commands.RemoveObject;

public class RemoveObjectCommandHandler(
    ISceneRepository sceneRepository)
    : IRequestHandler<RemoveObjectCommand, Result>
{
    public async Task<Result> Handle(RemoveObjectCommand command, CancellationToken ct)
    {
        var sceneId = SceneId.From(command.SceneId);
        var scene = await sceneRepository.GetByIdAsync(sceneId, ct);
        if (scene == null)
            return Result.Failure(Error.NotFound(nameof(Scene), sceneId));

        var objectId = SceneObjectId.From(command.ObjectId);
        var result = scene.RemoveObject(objectId);
        if (result.IsFailure)
            return result;

        await sceneRepository.SaveAsync(scene, ct);

        return Result.Success();
    }
}
