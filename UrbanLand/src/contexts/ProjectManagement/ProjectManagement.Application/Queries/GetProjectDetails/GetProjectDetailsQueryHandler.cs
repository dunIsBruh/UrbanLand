using MediatR;
using ProjectManagement.Domain.Repositories;
using ProjectManagement.Domain.Services;
using SharedKernel.Identity;
using SharedKernel.Primitives;

namespace ProjectManagement.Application.Queries.GetProjectDetails;

public class GetProjectDetailsQueryHandler(
    IProjectRepository projectRepository,
    ICurrentUserService currentUserService,
    IUserService userService)
    : IRequestHandler<GetProjectDetailsQuery, Result<ProjectDetailsDto>>
{
    public async Task<Result<ProjectDetailsDto>> Handle(GetProjectDetailsQuery query, CancellationToken ct)
    {
        var project = await projectRepository.GetByIdAsync(query.ProjectId, ct);
        if (project == null)
        {
            return Result<ProjectDetailsDto>.Failure(Error.NotFound("Project", query.ProjectId));
        }

        var userId = new UserId(currentUserService.UserId);
        var currentUserRole = project.Members
            .FirstOrDefault(m => m.MemberId == userId)?.Role.Name ?? "Visitor";

        var memberIds = project.Members
            .Select(m => m.MemberId)
            .Append(project.OwnerId)
            .Distinct()
            .ToList();

        var usersInfo = await userService.GetUsersInfoAsync(memberIds);

        var ownerInfo = usersInfo.GetValueOrDefault(project.OwnerId);
        var ownerName = ownerInfo?.DisplayName ?? project.OwnerId.Value.ToString();

        
        
        var membersDto = project.Members
            .Select(m => new ProjectDetailsAccessDto(
                m.MemberId.Value,
                usersInfo.GetValueOrDefault(m.MemberId)?.DisplayName ?? m.MemberId.Value.ToString(),
                m.Role.Name,
                m.JoinedAt
            )).ToList();
        
        var dto = new ProjectDetailsDto(
            project.Id.Value,
            project.Name,
            project.Description,
            project.Status.ToString(),
            project.Status.ToString(),
            project.OwnerId.Value,
            ownerName,
            project.CreatedAt,
            project.Members.Count,
            currentUserRole,
            new ProjectDetailsSettingsDto(1.0, true, "Flat", 10000),
            membersDto
        );

        return Result<ProjectDetailsDto>.Success(dto);
    }
}
