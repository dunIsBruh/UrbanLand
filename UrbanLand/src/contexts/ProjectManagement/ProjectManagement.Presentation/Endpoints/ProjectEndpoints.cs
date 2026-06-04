using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using ProjectManagement.Application.Commands.ArchiveProject;
using ProjectManagement.Application.Commands.CreateProject;
using ProjectManagement.Application.Commands.UpdateSettings;
using ProjectManagement.Application.Queries.GetProjectDetails;
using ProjectManagement.Application.Queries.GetUserProjects;
using ProjectManagement.Domain.ValueObjects;
using ProjectManagement.Presentation.Models.Project;
using ProjectManagement.Presentation.Models.ProjectMember;
using ProjectManagement.Presentation.Models.ProjectSettings;
using SharedKernel.Primitives;
using static ProjectManagement.Presentation.Endpoints.EndpointHelpers;
using ProjectId = SharedKernel.Contracts.ProjectId;

namespace ProjectManagement.Presentation.Endpoints;

public static class ProjectEndpoints
{
    public static IEndpointRouteBuilder MapProjectEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/projects")
            .WithTags("Projects")
            .RequireAuthorization();

        group.MapPost("/", CreateProjectAsync)
            .WithName("CreateProject")
            .WithDescription("Create a new project")
            .Produces<CreateProjectResponse>(StatusCodes.Status201Created)
            .Produces<Error>(StatusCodes.Status400BadRequest)
            .Produces<Error>(StatusCodes.Status401Unauthorized);

        group.MapPost("/{projectId:guid}/archive", ArchiveProjectAsync)
            .WithName("ArchiveProject")
            .WithDescription("Archive the project")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<Error>(StatusCodes.Status400BadRequest)
            .Produces<Error>(StatusCodes.Status403Forbidden)
            .Produces<Error>(StatusCodes.Status404NotFound);

        group.MapPut("/{projectId:guid}/settings", UpdateSettingsAsync)
            .WithName("UpdateSettings")
            .WithDescription("Update project settings")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<Error>(StatusCodes.Status400BadRequest)
            .Produces<Error>(StatusCodes.Status403Forbidden)
            .Produces<Error>(StatusCodes.Status404NotFound);

        group.MapGet("/", GetUserProjectsAsync)
            .WithName("GetUserProjects")
            .WithDescription("Get all projects for the current user")
            .Produces<List<UserProjectResponse>>();

        group.MapGet("/{projectId:guid}", GetProjectDetailsAsync)
            .WithName("GetProjectDetails")
            .WithDescription("Get detailed project information")
            .Produces<ProjectResponse>()
            .Produces<Error>(StatusCodes.Status403Forbidden)
            .Produces<Error>(StatusCodes.Status404NotFound);

        return app;
    }

    private static async Task<IResult> CreateProjectAsync(
        CreateProjectRequest request,
        IMediator mediator,
        HttpContext httpContext,
        CancellationToken ct)
    {
        var userId = GetUserId(httpContext);
        if (userId == Guid.Empty)
        {
            return TypedResults.Unauthorized();
        }

        var command = new CreateProjectCommand(request.Name, request.Type, request.Description);
        var result = await mediator.Send(command, ct);

        return result.Match<ProjectId, IResult>(
            onSuccess: projectId => TypedResults.Created(
                $"/api/projects/{projectId.Value}",
                new CreateProjectResponse
                {
                    ProjectId = projectId.Value,
                    Name = request.Name,
                    Type = request.Type,
                    CreatedAt = DateTime.UtcNow
                }),
            onFailure: error => error.Code switch
            {
                "Validation" => TypedResults.BadRequest(new Error(error.Code, error.Message)),
                _ => TypedResults.BadRequest(new Error("UNKNOWN_ERROR", error.Message))
            }
        );
    }

    private static async Task<IResult> ArchiveProjectAsync(
        Guid projectId,
        IMediator mediator,
        CancellationToken ct)
    {
        var command = new ArchiveProjectCommand(ProjectId.From(projectId));
        var result = await mediator.Send(command, ct);

        return result.Match(
            onSuccess: TypedResults.NoContent,
            onFailure: MapErrorToResponse
        );
    }

    private static async Task<IResult> UpdateSettingsAsync(
        Guid projectId,
        UpdateSettingsRequest request,
        IMediator mediator,
        CancellationToken ct)
    {
        var command = new UpdateSettingsCommand(
            ProjectId.From(projectId),
            request.DefaultGridSize,
            request.ShowGrid,
            request.DefaultTerrainType,
            request.MaxObjectsLimit,
            request.InvitationValidityHours);

        var result = await mediator.Send(command, ct);

        return result.Match(
            onSuccess: TypedResults.NoContent,
            onFailure: MapErrorToResponse
        );
    }

    private static async Task<IResult> GetUserProjectsAsync(
        IMediator mediator,
        HttpContext httpContext,
        CancellationToken ct,
        int page = 1,
        int pageSize = 20)
    {
        var query = new GetUserProjectsQuery(page, pageSize);
        var result = await mediator.Send(query, ct);

        var projects = result.Value;
        var response = projects.Select(p => new UserProjectResponse
        {
            Id = p.Id,
            Name = p.Name,
            Type = p.Type,
            Status = p.Status,
            UserRole = p.AccessLevel,
            MemberCount = p.ObserverCount,
            CreatedAt = p.CreatedAt,
            LastModifiedAt = p.LastModifiedAt
        }).ToList();

        return TypedResults.Ok(response);
    }

    private static async Task<IResult> GetProjectDetailsAsync(
        Guid projectId,
        IMediator mediator,
        CancellationToken ct)
    {
        var query = new GetProjectDetailsQuery(ProjectId.From(projectId));
        var result = await mediator.Send(query, ct);

        return result.Match<ProjectDetailsDto, IResult>(
            onSuccess: dto => TypedResults.Ok(new ProjectResponse
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Type = dto.Type,
                Status = dto.Status,
                OwnerId = dto.OwnerId,
                OwnerName = dto.OwnerName,
                CreatedAt = dto.CreatedAt,
                MemberCount = dto.ObserverCount,
                CurrentUserRole = dto.CurrentUserAccessLevel,
                Settings = new ProjectSettingsResponse
                {
                    DefaultGridSize = dto.Settings.DefaultGridSize,
                    ShowGrid = dto.Settings.ShowGrid,
                    DefaultTerrainType = dto.Settings.DefaultTerrainType,
                    MaxObjectsLimit = dto.Settings.MaxObjectsLimit
                },
                Members = dto.AccessList.Select(a => new MemberResponse
                {
                    UserId = a.UserId,
                    UserName = a.UserName,
                    Role = a.AccessLevel,
                    JoinedAt = a.GrantedAt
                }).ToList()
            }),
            onFailure: error => error.Code switch
            {
                "NotFound" => TypedResults.NotFound(new Error(error.Code, error.Message)),
                _ => TypedResults.BadRequest(new Error(error.Code, error.Message))
            }
        );
    }
}
