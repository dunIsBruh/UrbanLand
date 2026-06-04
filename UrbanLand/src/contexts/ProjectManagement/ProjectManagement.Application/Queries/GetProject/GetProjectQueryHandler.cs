using MediatR;
using ProjectManagement.Domain.Repositories;
using SharedKernel.Primitives;

namespace ProjectManagement.Application.Queries.GetProject;

public class GetProjectQueryHandler : IRequestHandler<GetProjectQuery, Result<ProjectDto>>
{
    private readonly IProjectRepository _projectRepository;

    public GetProjectQueryHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<Result<ProjectDto>> Handle(GetProjectQuery query, CancellationToken ct)
    {
        var project = await _projectRepository.GetByIdAsync(query.ProjectId, ct);
        if (project == null)
        {
            return Result<ProjectDto>.Failure(Error.NotFound("Project", query.ProjectId));
        }

        var dto = new ProjectDto(
            project.Id,
            project.Name,
            project.OwnerId,
            project.Status,
            project.CreatedAt,
            project.Members.Count);

        return Result<ProjectDto>.Success(dto);
    }
}
