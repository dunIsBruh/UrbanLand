using SceneDesign.Domain.Entities;
using SceneDesign.Domain.ValueObjects;
using SharedKernel.Primitives;

namespace SceneDesign.Domain.Services;

public interface ISceneValidationService
{
    Result ValidatePlacement(Scene scene, Position3D position, AssetId assetId);
}