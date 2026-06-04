using MediatR;
using SharedKernel.Primitives;

namespace AssetCatalog.Application.Commands.ApproveAsset;

public record ApproveAssetCommand(Guid AssetId) : IRequest<Result>;
