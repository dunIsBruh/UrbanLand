using AssetCatalog.Domain.ValueObjects;
using MediatR;
using Core.Primitives;

namespace AssetCatalog.Application.Commands.ImportCustomAsset;

public record ImportCustomAssetCommand(
    string Name,
    string? Description,
    string FileUrl,
    string FileName,
    long FileSize,
    string Format,
    double Width,
    double Height,
    double Depth,
    int PolygonCount) : IRequest<Result<AssetId>>;
