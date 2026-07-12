using Core.Primitives;
using SceneDesign.Domain.Entities;
using SceneDesign.Domain.Services;
using SceneDesign.Domain.ValueObjects;

namespace SceneDesign.Infrastructure.Services;

public class SceneValidationService : ISceneValidationService
{
    public Result ValidatePlacement(Scene scene, Position3D position, AssetId assetId)
    {
        if (scene.Settings.DetectCollisions)
        {
            var newBounds = new BoundingBox(position, Scale.Default);

            foreach (var obj in scene.Objects)
            {
                if (obj.BoundingBox.Intersects(newBounds))
                {
                    return Result.Failure(Error.Validation("Position collides with existing object"));
                }
            }
        }

        return Result.Success();
    }
}
