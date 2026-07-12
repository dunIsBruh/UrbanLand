using Core.Primitives;
using SceneDesign.Domain.Entities;
using SceneDesign.Domain.ValueObjects;

namespace SceneDesign.Domain.Services;

public interface ISceneValidationService
{
    Result ValidatePlacement(Scene scene, Position3D position, AssetId assetId);
}