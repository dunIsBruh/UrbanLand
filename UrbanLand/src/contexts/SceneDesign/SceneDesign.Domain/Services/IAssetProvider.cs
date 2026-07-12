using Core.Primitives;
using SceneDesign.Domain.ValueObjects;

namespace SceneDesign.Domain.Services;

public interface IAssetProvider
{
    Task<Result<SceneAsset>> GetAssetAsync(AssetId assetId);
}

// Shift this
public record SceneAsset(AssetId Id, string Name, Dimensions3D Dimensions);

public record Dimensions3D(double Width, double Height, double Depth);