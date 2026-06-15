using MediatR;
using ProjectManagement.Domain.Repositories;
using SharedKernel.Identity;
using SharedKernel.Primitives;

namespace ProjectManagement.Application.Queries.GetUserProjects;

public class GetUserProjectsQueryHandler : IRequestHandler<GetUserProjectsQuery, Result<List<UserProjectDto>>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public GetUserProjectsQueryHandler(
        IProjectRepository projectRepository,
        ICurrentUserAccessor currentUserAccessor)
    {
        _projectRepository = projectRepository;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<Result<List<UserProjectDto>>> Handle(GetUserProjectsQuery query, CancellationToken ct)
    {
        var userId = new UserId(_currentUserAccessor.UserId);
        var projects = await _projectRepository.GetByUserIdAsync(userId, ct);

        var dtos = projects.Select(p => new UserProjectDto(
            p.Id.Value,
            p.Name,
            p.Status.ToString(),
            p.Status.ToString(),
            p.Members.FirstOrDefault(m => m.MemberId == userId)?.Role.Name ?? "Visitor",
            p.Members.Count,
            p.CreatedAt,
            null
        )).ToList();

        return Result<List<UserProjectDto>>.Success(dtos);
    }
}
