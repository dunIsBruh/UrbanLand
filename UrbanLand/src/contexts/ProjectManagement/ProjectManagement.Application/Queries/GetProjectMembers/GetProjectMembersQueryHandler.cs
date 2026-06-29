using Core.Primitives;
using MediatR;
using ProjectManagement.Domain.Repositories;
using Core.Primitives;

namespace ProjectManagement.Application.Queries.GetProjectMembers;

public class GetProjectMembersQueryHandler : IRequestHandler<GetProjectMembersQuery, Result<List<ProjectMemberDto>>>
{
    private readonly IProjectRepository _projectRepository;

    public GetProjectMembersQueryHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<Result<List<ProjectMemberDto>>> Handle(GetProjectMembersQuery query, CancellationToken ct)
    {
        var project = await _projectRepository.GetByIdAsync(query.ProjectId, ct);
        if (project == null)
        {
            return Result<List<ProjectMemberDto>>.Failure(Error.NotFound("Project", query.ProjectId));
        }

        var members = project.Members
            .Select(m => new ProjectMemberDto(m.MemberId, m.Role.Name, m.JoinedAt))
            .ToList();

        return Result<List<ProjectMemberDto>>.Success(members);
    }
}
