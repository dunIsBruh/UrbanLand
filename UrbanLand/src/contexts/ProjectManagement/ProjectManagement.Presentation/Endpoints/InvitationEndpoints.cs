using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using ProjectManagement.Application.Commands.AcceptInvitation;
using ProjectManagement.Application.Commands.CreateInvitation;
using ProjectManagement.Domain.ValueObjects;
using ProjectManagement.Presentation.Models.Invitation;
using ProjectManagement.Presentation.Models.ProjectMember;
using SharedKernel.Identity;
using SharedKernel.Primitives;
using static ProjectManagement.Presentation.Endpoints.EndpointHelpers;
using ProjectId = SharedKernel.Contracts.ProjectId;

namespace ProjectManagement.Presentation.Endpoints;

public static class InvitationEndpoints
{
    public static IEndpointRouteBuilder MapInvitationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/projects/{projectId:guid}/invitations")
            .WithTags("Invitations")
            .RequireAuthorization();

        group.MapPost("/", CreateInvitationAsync)
            .WithName("CreateInvitation")
            .WithDescription("Create an invitation link")
            .Produces<InvitationResponse>(StatusCodes.Status201Created)
            .Produces<Error>(StatusCodes.Status400BadRequest)
            .Produces<Error>(StatusCodes.Status403Forbidden)
            .Produces<Error>(StatusCodes.Status404NotFound);

        group.MapPost("/accept", AcceptInvitationAsync)
            .WithName("AcceptInvitation")
            .WithDescription("Accept an invitation to join the project via invite code")
            .Produces<MemberResponse>()
            .Produces<Error>(StatusCodes.Status400BadRequest)
            .Produces<Error>(StatusCodes.Status404NotFound);

        return app;
    }

    private static async Task<IResult> CreateInvitationAsync(
        Guid projectId,
        CreateInvitationRequest request,
        IMediator mediator,
        CancellationToken ct)
    {
        var command = new CreateInvitationCommand(
            ProjectId.From(projectId),
            ProjectRole.FromName(request.SuggestedRole));

        var result = await mediator.Send(command, ct);

        return result.Match<CreateInvitationResult, IResult>(
            onSuccess: invitation => TypedResults.Created(
                $"/api/projects/{projectId}/invitations/{invitation.Id}",
                new InvitationResponse
                {
                    Id = invitation.Id,
                    InviteCode = invitation.InviteCode,
                    SuggestedRole = invitation.SuggestedRole,
                    CreatedAt = invitation.CreatedAt,
                    ExpiresAt = invitation.ExpiresAt
                }),
            onFailure: MapErrorToResponse
        );
    }

    private static async Task<IResult> AcceptInvitationAsync(
        Guid projectId,
        AcceptInvitationRequest request,
        IMediator mediator,
        HttpContext httpContext,
        CancellationToken ct)
    {
        var userId = GetUserId(httpContext);

        var command = new AcceptInvitationCommand(
            ProjectId.From(projectId),
            request.InviteCode,
            UserId.From(userId));

        var result = await mediator.Send(command, ct);

        return result.Match<string, IResult>(
            onSuccess: role => TypedResults.Ok(new MemberResponse
            {
                UserId = userId,
                Role = role,
                JoinedAt = DateTime.UtcNow
            }),
            onFailure: error => TypedResults.BadRequest(new Error(error.Code, error.Message))
        );
    }
}
