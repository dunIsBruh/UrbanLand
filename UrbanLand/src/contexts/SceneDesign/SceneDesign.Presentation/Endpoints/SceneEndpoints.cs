using Core.Primitives;
using Core.Web;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SceneDesign.Application.Commands.CreateScene;
using SceneDesign.Application.Commands.MoveObject;
using SceneDesign.Application.Commands.PlaceObject;
using SceneDesign.Application.Commands.RemoveObject;
using SceneDesign.Application.Commands.SwitchViewMode;
using SceneDesign.Application.Queries.GetScene;
using SceneDesign.Domain.Enums;
using SceneDesign.Presentation.Models.Requests;
using SceneDesign.Presentation.Models.Responses;

namespace SceneDesign.Presentation.Endpoints;

public static class SceneEndpoints
{
    public static IEndpointRouteBuilder MapSceneEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api")
            .WithTags("Scenes")
            .RequireAuthorization();

        group.MapPost("/projects/{projectId:guid}/scene", CreateSceneAsync)
            .WithName("CreateScene")
            .WithSummary("Create Scene")
            .WithDescription("Create a scene for a project")
            .Produces<CreateSceneResponse>(StatusCodes.Status201Created)
            .Produces<Error>(StatusCodes.Status400BadRequest)
            .Produces<Error>(StatusCodes.Status409Conflict)
            .Produces<Error>(StatusCodes.Status401Unauthorized);

        group.MapGet("/projects/{projectId:guid}/scene", GetSceneAsync)
            .WithName("GetScene")
            .WithSummary("Get Scene")
            .WithDescription("Get scene by project ID")
            .Produces<SceneResponse>()
            .Produces<Error>(StatusCodes.Status404NotFound);

        group.MapPost("/scenes/{sceneId:guid}/objects", PlaceObjectAsync)
            .WithName("PlaceObject")
            .WithSummary("Place Object")
            .WithDescription("Place an object in the scene")
            .Produces<PlaceObjectResponse>(StatusCodes.Status201Created)
            .Produces<Error>(StatusCodes.Status400BadRequest)
            .Produces<Error>(StatusCodes.Status404NotFound)
            .Produces<Error>(StatusCodes.Status409Conflict);

        group.MapPut("/scenes/{sceneId:guid}/objects/{objectId:guid}/position", MoveObjectAsync)
            .WithName("MoveObject")
            .WithSummary("Move Object")
            .WithDescription("Move an object in the scene")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<Error>(StatusCodes.Status400BadRequest)
            .Produces<Error>(StatusCodes.Status404NotFound);

        group.MapDelete("/scenes/{sceneId:guid}/objects/{objectId:guid}", RemoveObjectAsync)
            .WithName("RemoveObject")
            .WithSummary("Remove Object")
            .WithDescription("Remove an object from the scene")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<Error>(StatusCodes.Status404NotFound);

        group.MapPut("/scenes/{sceneId:guid}/viewmode", SwitchViewModeAsync)
            .WithName("SwitchViewMode")
            .WithSummary("Switch View Mode")
            .WithDescription("Switch scene view mode between 2D and 3D")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<Error>(StatusCodes.Status400BadRequest)
            .Produces<Error>(StatusCodes.Status404NotFound);

        return app;
    }

    private static async Task<IResult> CreateSceneAsync(
        Guid projectId,
        IMediator mediator,
        CancellationToken ct)
    {
        var command = new CreateSceneCommand(projectId);
        var result = await mediator.Send(command, ct);

        return result.Match<Domain.ValueObjects.SceneId, IResult>(
            onSuccess: sceneId => TypedResults.Created(
                $"/api/scenes/{sceneId.Value}",
                new CreateSceneResponse { SceneId = sceneId.Value, ProjectId = projectId }),
            onFailure: error => error.Code switch
            {
                "Conflict" => TypedResults.Conflict(new Error(error.Code, error.Message)),
                _ => TypedResults.BadRequest(new Error(error.Code, error.Message))
            }
        );
    }

    private static async Task<IResult> GetSceneAsync(
        Guid projectId,
        IMediator mediator,
        CancellationToken ct)
    {
        var query = new GetSceneQuery(projectId);
        var result = await mediator.Send(query, ct);

        return result.Match<SceneDto, IResult>(
            onSuccess: dto => TypedResults.Ok(SceneResponse.FromDto(dto)),
            onFailure: EndpointMapper.MapErrorToResponse
        );
    }

    private static async Task<IResult> PlaceObjectAsync(
        Guid sceneId,
        PlaceObjectRequest request,
        IMediator mediator,
        CancellationToken ct)
    {
        var command = new PlaceObjectCommand(
            sceneId,
            request.AssetId,
            request.PositionX,
            request.PositionY,
            request.PositionZ,
            request.RotationYaw,
            request.RotationPitch,
            request.RotationRoll,
            request.ScaleX,
            request.ScaleY,
            request.ScaleZ,
            request.LayerName);

        var result = await mediator.Send(command, ct);

        return result.Match<Domain.Entities.SceneObject, IResult>(
            onSuccess: obj => TypedResults.Created(
                $"/api/scenes/{sceneId}/objects/{obj.Id.Value}",
                new PlaceObjectResponse { ObjectId = obj.Id.Value }),
            onFailure: error => error.Code switch
            {
                "Conflict" => TypedResults.Conflict(new Error(error.Code, error.Message)),
                "NotFound" => TypedResults.NotFound(new Error(error.Code, error.Message)),
                _ => TypedResults.BadRequest(new Error(error.Code, error.Message))
            }
        );
    }

    private static async Task<IResult> MoveObjectAsync(
        Guid sceneId,
        Guid objectId,
        MoveObjectRequest request,
        IMediator mediator,
        CancellationToken ct)
    {
        var command = new MoveObjectCommand(
            sceneId,
            objectId,
            request.NewPositionX,
            request.NewPositionY,
            request.NewPositionZ);

        var result = await mediator.Send(command, ct);

        return result.Match(
            onSuccess: TypedResults.NoContent,
            onFailure: EndpointMapper.MapErrorToResponse
        );
    }

    private static async Task<IResult> RemoveObjectAsync(
        Guid sceneId,
        Guid objectId,
        IMediator mediator,
        CancellationToken ct)
    {
        var command = new RemoveObjectCommand(sceneId, objectId);
        var result = await mediator.Send(command, ct);

        return result.Match(
            onSuccess: TypedResults.NoContent,
            onFailure: EndpointMapper.MapErrorToResponse
        );
    }

    private static async Task<IResult> SwitchViewModeAsync(
        Guid sceneId,
        SwitchViewModeRequest request,
        IMediator mediator,
        CancellationToken ct)
    {
        if (!Enum.TryParse<ViewMode>(request.ViewMode, true, out var viewMode))
        {
            return TypedResults.BadRequest(new Error("Validation", $"Invalid view mode: {request.ViewMode}"));
        }

        var command = new SwitchViewModeCommand(sceneId, viewMode);
        var result = await mediator.Send(command, ct);

        return result.Match(
            onSuccess: TypedResults.NoContent,
            onFailure: EndpointMapper.MapErrorToResponse
        );
    }
}
