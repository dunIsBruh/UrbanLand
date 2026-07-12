using AssetCatalog.Domain.Entities;
using AssetCatalog.Domain.Repositories;
using AssetCatalog.Domain.ValueObjects;
using MassTransit;
using MediatR;
using Core.Identity;
using Core.IntegrationEvents.AssetCatalog;
using Core.Primitives;

namespace AssetCatalog.Application.Commands.ImportCustomAsset;

public class ImportCustomAssetCommandHandler(
    IAssetRepository assetRepository,
    IPublishEndpoint publishEndpoint,
    ICurrentUserAccessor currentUserAccessor)
    : IRequestHandler<ImportCustomAssetCommand, Result<AssetId>>
{
    public async Task<Result<AssetId>> Handle(ImportCustomAssetCommand command, CancellationToken ct)
    {
        var uploaderId = new UserId(currentUserAccessor.UserId);

        var dimensions = new Dimensions(command.Width, command.Height, command.Depth);
        var modelData = new ModelData(
            command.FileUrl, command.FileName, command.FileSize,
            command.Format, dimensions, command.PolygonCount);

        var result = AssetTemplate.ImportCustom(command.Name, command.Description, uploaderId, modelData);
        if (result.IsFailure)
            return Result<AssetId>.Failure(result.Error);

        var asset = result.Value;
        await assetRepository.SaveAsync(asset, ct);

        await publishEndpoint.Publish(
            new AssetImportedIntegrationEvent(asset.Id.Value, asset.Name, uploaderId.Value),
            ct);

        return Result<AssetId>.Success(asset.Id);
    }
}
