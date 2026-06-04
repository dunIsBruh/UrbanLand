using AssetCatalog.Domain.Entities;
using AssetCatalog.Domain.Repositories;
using AssetCatalog.Domain.ValueObjects;
using MassTransit;
using MediatR;
using SharedKernel.Identity;
using SharedKernel.IntegrationEvents.AssetCatalog;
using SharedKernel.Primitives;
using AssetCatalog.Application.Services;

namespace AssetCatalog.Application.Commands.ImportFromSketchfab;

public class ImportFromSketchfabCommandHandler(
    ISketchfabService sketchfabService,
    IAssetRepository assetRepository,
    IPublishEndpoint publishEndpoint,
    ICurrentUserService currentUserService)
    : IRequestHandler<ImportFromSketchfabCommand, Result<AssetId>>
{
    public async Task<Result<AssetId>> Handle(ImportFromSketchfabCommand command, CancellationToken ct)
    {
        var uploaderId = new UserId(currentUserService.UserId);

        var modelData = await sketchfabService.DownloadModelAsync(command.SketchfabModelId);
        if (modelData == null)
            return Result<AssetId>.Failure(Error.Internal("Failed to download model from Sketchfab"));

        var result = AssetTemplate.ImportCustom(
            command.Name, command.Description, uploaderId, modelData);
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
