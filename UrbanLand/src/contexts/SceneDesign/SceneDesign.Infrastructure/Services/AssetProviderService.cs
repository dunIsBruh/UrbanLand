using Core.IntegrationEvents.SceneDesign;
using Core.Primitives;
using MassTransit;
using Microsoft.Extensions.Logging;
using SceneDesign.Domain.Services;
using SceneDesign.Domain.ValueObjects;

namespace SceneDesign.Infrastructure.Services;

public class AssetProviderService(
    IRequestClient<GetAssetInfoRequest> requestClient,
    ILogger<AssetProviderService> logger)
    : IAssetProvider
{
    public async Task<Result<SceneAsset>> GetAssetAsync(AssetId assetId)
    {
        try
        {
            var response = await requestClient.GetResponse<GetAssetInfoResponse>(
                new GetAssetInfoRequest(assetId.Value));

            var msg = response.Message;
            return Result<SceneAsset>.Success(new SceneAsset(
                new AssetId(msg.AssetId),
                msg.Name,
                new Dimensions3D(msg.Width, msg.Height, msg.Depth)));
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex,
                "Failed to fetch asset {AssetId} from AssetCatalog, using defaults",
                assetId.Value);

            return Result<SceneAsset>.Success(new SceneAsset(
                assetId,
                "Unknown Asset",
                new Dimensions3D(1, 1, 1)));
        }
    }
}
