using AssetCatalog.Application.Commands.ApproveAsset;
using AssetCatalog.Application.Commands.ImportCustomAsset;
using AssetCatalog.Application.Commands.ImportFromSketchfab;
using AssetCatalog.Application.Queries.GetAssetsByCategory;
using AssetCatalog.Application.Queries.GetTopographicSymbols;
using AssetCatalog.Domain.Enums;
using AssetDto = AssetCatalog.Application.Queries.GetAsset.AssetDto;
using GetAssetQuery = AssetCatalog.Application.Queries.GetAsset.GetAssetQuery;
using TopographicSymbolDto = AssetCatalog.Application.Queries.GetTopographicSymbols.TopographicSymbolDto;
using AssetCatalog.Domain.ValueObjects;
using AssetCatalog.Presentation.Models;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SharedKernel.Primitives;
using static AssetCatalog.Presentation.Endpoints.EndpointHelpers;

namespace AssetCatalog.Presentation.Endpoints;

public static class AssetEndpoints
{
    public static IEndpointRouteBuilder MapAssetEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/assets")
            .WithTags("Assets")
            .RequireAuthorization();

        group.MapGet("/", GetAssetsAsync)
            .WithName("GetAssets")
            .WithSummary("Get Assets")
            .WithDescription("Get assets by category")
            .Produces<List<AssetSummaryResponse>>();

        group.MapGet("/{assetId:guid}", GetAssetAsync)
            .WithName("GetAsset")
            .WithSummary("Get Asset")
            .WithDescription("Get asset details")
            .Produces<AssetDetailResponse>()
            .Produces<Error>(StatusCodes.Status404NotFound);

        group.MapPost("/import", ImportCustomAssetAsync)
            .WithName("ImportCustomAsset")
            .WithSummary("Import Custom Asset")
            .WithDescription("Import a custom 3D model")
            .Produces<ImportAssetResponse>(StatusCodes.Status201Created)
            .Produces<Error>(StatusCodes.Status400BadRequest);

        group.MapPost("/sketchfab/import", ImportFromSketchfabAsync)
            .WithName("ImportFromSketchfab")
            .WithSummary("Import From Sketchfab")
            .WithDescription("Import a model from Sketchfab")
            .Produces<ImportAssetResponse>(StatusCodes.Status201Created)
            .Produces<Error>(StatusCodes.Status400BadRequest);

        group.MapPut("/{assetId:guid}/approve", ApproveAssetAsync)
            .WithName("ApproveAsset")
            .WithSummary("Approve Asset")
            .WithDescription("Approve a pending asset")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<Error>(StatusCodes.Status404NotFound);

        group.MapGet("/topographic-symbols", GetTopographicSymbolsAsync)
            .WithName("GetTopographicSymbols")
            .WithSummary("Get Topographic Symbols")
            .WithDescription("Get all topographic symbols for 2D mode")
            .Produces<List<TopographicSymbolResponse>>();

        return app;
    }

    private static async Task<IResult> GetAssetsAsync(
        IMediator mediator,
        AssetCategory? category,
        CancellationToken ct)
    {
        if (category == null)
        {
            var all = await mediator.Send(new GetAssetsByCategoryQuery(AssetCategory.Vegetation), ct);
            return TypedResults.Ok(all.Value.Select(AssetSummaryResponse.FromDto).ToList());
        }

        var query = new GetAssetsByCategoryQuery(category.Value);
        var result = await mediator.Send(query, ct);

        return result.Match<List<AssetSummaryDto>, IResult>(
            onSuccess: list => TypedResults.Ok(list.Select(AssetSummaryResponse.FromDto).ToList()),
            onFailure: MapErrorToResponse
        );
    }

    private static async Task<IResult> GetAssetAsync(
        Guid assetId,
        IMediator mediator,
        CancellationToken ct)
    {
        var query = new GetAssetQuery(assetId);
        var result = await mediator.Send(query, ct);

        return result.Match<AssetDto, IResult>(
            onSuccess: dto => TypedResults.Ok(AssetDetailResponse.FromDto(dto)),
            onFailure: MapErrorToResponse
        );
    }

    private static async Task<IResult> ImportCustomAssetAsync(
        ImportCustomAssetRequest request,
        IMediator mediator,
        CancellationToken ct)
    {
        var command = new ImportCustomAssetCommand(
            request.Name, request.Description,
            request.FileUrl, request.FileName, request.FileSize,
            request.Format, request.Width, request.Height, request.Depth,
            request.PolygonCount);

        var result = await mediator.Send(command, ct);

        return result.Match<AssetId, IResult>(
            onSuccess: assetId => TypedResults.Created(
                $"/api/assets/{assetId.Value}",
                new ImportAssetResponse { AssetId = assetId.Value }),
            onFailure: MapErrorToResponse
        );
    }

    private static async Task<IResult> ImportFromSketchfabAsync(
        ImportFromSketchfabRequest request,
        IMediator mediator,
        CancellationToken ct)
    {
        var command = new ImportFromSketchfabCommand(
            request.SketchfabModelId, request.Name, request.Description);

        var result = await mediator.Send(command, ct);

        return result.Match<AssetId, IResult>(
            onSuccess: assetId => TypedResults.Created(
                $"/api/assets/{assetId.Value}",
                new ImportAssetResponse { AssetId = assetId.Value }),
            onFailure: MapErrorToResponse
        );
    }

    private static async Task<IResult> ApproveAssetAsync(
        Guid assetId,
        IMediator mediator,
        CancellationToken ct)
    {
        var command = new ApproveAssetCommand(assetId);
        var result = await mediator.Send(command, ct);

        return result.Match(
            onSuccess: TypedResults.NoContent,
            onFailure: MapErrorToResponse
        );
    }

    private static async Task<IResult> GetTopographicSymbolsAsync(
        IMediator mediator,
        CancellationToken ct)
    {
        var query = new GetTopographicSymbolsQuery();
        var result = await mediator.Send(query, ct);

        return result.Match<List<TopographicSymbolDto>, IResult>(
            onSuccess: list => TypedResults.Ok(list.Select(t => new TopographicSymbolResponse
            {
                SymbolType = t.SymbolType,
                Color = t.Color,
                Size = t.Size
            }).ToList()),
            onFailure: MapErrorToResponse
        );
    }
}
