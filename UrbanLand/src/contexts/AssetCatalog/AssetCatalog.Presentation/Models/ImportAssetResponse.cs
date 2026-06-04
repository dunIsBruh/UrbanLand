namespace AssetCatalog.Presentation.Models;

/// <summary>
/// Response returned after an asset import operation.
/// </summary>
public sealed record ImportAssetResponse
{
    /// <summary>
    /// Unique identifier of the imported asset.
    /// </summary>
    public Guid AssetId { get; init; }
}
