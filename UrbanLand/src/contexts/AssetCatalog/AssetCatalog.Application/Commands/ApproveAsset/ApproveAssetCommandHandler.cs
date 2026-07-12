using AssetCatalog.Domain.Entities;
using AssetCatalog.Domain.Repositories;
using AssetCatalog.Domain.ValueObjects;
using MediatR;
using Core.Primitives;

namespace AssetCatalog.Application.Commands.ApproveAsset;

public class ApproveAssetCommandHandler(
    IAssetRepository assetRepository)
    : IRequestHandler<ApproveAssetCommand, Result>
{
    public async Task<Result> Handle(ApproveAssetCommand command, CancellationToken ct)
    {
        var assetId = AssetId.From(command.AssetId);
        var asset = await assetRepository.GetByIdAsync(assetId, ct);
        if (asset == null)
            return Result.Failure(Error.NotFound(nameof(AssetTemplate), assetId));

        asset.Approve();
        await assetRepository.SaveAsync(asset, ct);
        return Result.Success();
    }
}
