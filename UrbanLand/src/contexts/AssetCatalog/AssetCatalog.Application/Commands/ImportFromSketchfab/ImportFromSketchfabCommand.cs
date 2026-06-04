using AssetCatalog.Domain.ValueObjects;
using MediatR;
using SharedKernel.Primitives;

namespace AssetCatalog.Application.Commands.ImportFromSketchfab;

public record ImportFromSketchfabCommand(
    string SketchfabModelId,
    string Name,
    string? Description) : IRequest<Result<AssetId>>;
