using MediatR;
using SceneDesign.Domain.Entities;
using SceneDesign.Domain.Repositories;
using SceneDesign.Domain.Services;
using SceneDesign.Domain.ValueObjects;
using SharedKernel.Primitives;

namespace SceneDesign.Application.Commands.PlaceObject;

public class PlaceObjectCommandHandler(
    ISceneRepository sceneRepository,
    IAssetProvider assetProvider)
    : IRequestHandler<PlaceObjectCommand, Result<SceneObject>>
{
    public async Task<Result<SceneObject>> Handle(PlaceObjectCommand command, CancellationToken ct)
    {
        var sceneId = SceneId.From(command.SceneId);
        var scene = await sceneRepository.GetByIdAsync(sceneId, ct);
        if (scene == null)
        {
            return Result<SceneObject>.Failure(Error.NotFound(nameof(Scene), sceneId));
        }

        var assetId = new AssetId(command.AssetId);
        var position = new Position3D(command.PositionX, command.PositionY, command.PositionZ);
        var rotation = new Rotation(command.RotationYaw, command.RotationPitch, command.RotationRoll);
        var scale = new Scale(command.ScaleX, command.ScaleY, command.ScaleZ);

        var assetResult = await assetProvider.GetAssetAsync(assetId);
        if (assetResult.IsFailure)
        {
            return Result<SceneObject>.Failure(assetResult.Error);
        }

        var result = scene.PlaceObject(assetId, position, rotation, scale, command.LayerName);
        if (result.IsFailure)
        {
            return Result<SceneObject>.Failure(result.Error);
        }

        await sceneRepository.SaveAsync(scene, ct);

        return result;
    }
}
