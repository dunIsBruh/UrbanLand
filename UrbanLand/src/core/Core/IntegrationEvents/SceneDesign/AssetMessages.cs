namespace Core.IntegrationEvents.SceneDesign;

public record GetAssetInfoRequest(Guid AssetId);

public record GetAssetInfoResponse(Guid AssetId, string Name, double Width, double Height, double Depth);
