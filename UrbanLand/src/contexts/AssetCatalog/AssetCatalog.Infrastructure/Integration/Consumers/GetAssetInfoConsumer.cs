using AssetCatalog.Domain.Repositories;
using AssetCatalog.Domain.ValueObjects;
using MassTransit;
using Microsoft.Extensions.Logging;
using Core.IntegrationEvents.SceneDesign;

namespace AssetCatalog.Infrastructure.Integration.Consumers;

public class GetAssetInfoConsumer(
    IAssetRepository assetRepository,
    ILogger<GetAssetInfoConsumer> logger)
    : IConsumer<GetAssetInfoRequest>
{
    public async Task Consume(ConsumeContext<GetAssetInfoRequest> context)
    {
        var assetId = new AssetId(context.Message.AssetId);
        var asset = await assetRepository.GetByIdAsync(assetId, context.CancellationToken);

        if (asset == null)
        {
            logger.LogWarning("Asset {AssetId} not found for GetAssetInfo", context.Message.AssetId);
            await context.RespondAsync(new GetAssetInfoResponse(
                context.Message.AssetId, "Unknown", 1, 1, 1));
            return;
        }

        var current = asset.CurrentVersion;
        await context.RespondAsync(new GetAssetInfoResponse(
            context.Message.AssetId,
            asset.Name,
            current.Model.Dimensions.Width,
            current.Model.Dimensions.Height,
            current.Model.Dimensions.Depth));
    }
}
