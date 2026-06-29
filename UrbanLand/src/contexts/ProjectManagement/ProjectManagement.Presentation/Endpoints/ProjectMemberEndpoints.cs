using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using ProjectManagement.Application.Commands.AddMember;
using ProjectManagement.Application.Commands.ChangeMemberRole;
using ProjectManagement.Application.Commands.RemoveMember;
using ProjectManagement.Domain.ValueObjects;
using ProjectManagement.Presentation.Models.ProjectMember;
using Core.Identity;
using Core.Primitives;
using Core.Web;
using ProjectId = Core.Contracts.ProjectId;

namespace ProjectManagement.Presentation.Endpoints;

public static class ProjectMemberEndpoints
{
    public static IEndpointRouteBuilder MapProjectMemberEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/projects/{projectId:guid}/members")
            .WithTags("Project Members")
            .RequireAuthorization();

        group.MapPost("/", AddMemberAsync)
            .WithName("AddMember")
            .WithSummary("Add Member")
            .WithDescription("Add a member to the project")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<Error>(StatusCodes.Status400BadRequest)
            .Produces<Error>(StatusCodes.Status403Forbidden)
            .Produces<Error>(StatusCodes.Status404NotFound);

        group.MapPut("/{userId:guid}/role", ChangeMemberRoleAsync)
            .WithName("ChangeMemberRole")
            .WithSummary("Change Member Role")
            .WithDescription("Change a member's role")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<Error>(StatusCodes.Status400BadRequest)
            .Produces<Error>(StatusCodes.Status403Forbidden)
            .Produces<Error>(StatusCodes.Status404NotFound);

        group.MapDelete("/{userId:guid}", RemoveMemberAsync)
            .WithName("RemoveMember")
            .WithSummary("Remove Member")
            .WithDescription("Remove a member from the project")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<Error>(StatusCodes.Status400BadRequest)
            .Produces<Error>(StatusCodes.Status403Forbidden)
            .Produces<Error>(StatusCodes.Status404NotFound);

        return app;
    }

    private static async Task<IResult> AddMemberAsync(
        Guid projectId,
        AddMemberRequest request,
        IMediator mediator,
        CancellationToken ct)
    {
        var command = new AddMemberCommand(
            ProjectId.From(projectId),
            UserId.From(request.UserId),
            ProjectRole.FromName(request.Role));

        var result = await mediator.Send(command, ct);

        return result.Match(
            onSuccess: TypedResults.NoContent,
            onFailure: EndpointMapper.MapErrorToResponse
        );
    }

    private static async Task<IResult> ChangeMemberRoleAsync(
        Guid projectId,
        Guid userId,
        ChangeMemberRoleRequest request,
        IMediator mediator,
        CancellationToken ct)
    {
        var command = new ChangeMemberRoleCommand(
            ProjectId.From(projectId),
            UserId.From(userId),
            ProjectRole.FromName(request.NewRole));

        var result = await mediator.Send(command, ct);

        return result.Match(
            onSuccess: TypedResults.NoContent,
            onFailure: EndpointMapper.MapErrorToResponse
        );
    }

    private static async Task<IResult> RemoveMemberAsync(
        Guid projectId,
        Guid userId,
        IMediator mediator,
        CancellationToken ct)
    {
        var command = new RemoveMemberCommand(
            ProjectId.From(projectId),
            UserId.From(userId));

        var result = await mediator.Send(command, ct);

        return result.Match(
            onSuccess: TypedResults.NoContent,
            onFailure: EndpointMapper.MapErrorToResponse
        );
    }
}
