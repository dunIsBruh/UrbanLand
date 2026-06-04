using MediatR;
using ProjectManagement.Domain.Repositories;
using SharedKernel.Identity;
using SharedKernel.Primitives;

namespace ProjectManagement.Application.Queries.GetUserProjects;

public class GetUserProjectsQueryHandler : IRequestHandler<GetUserProjectsQuery, Result<List<UserProjectDto>>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetUserProjectsQueryHandler(
        IProjectRepository projectRepository,
        ICurrentUserService currentUserService)
    {
        _projectRepository = projectRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Result<List<UserProjectDto>>> Handle(GetUserProjectsQuery query, CancellationToken ct)
    {
        var userId = new UserId(_currentUserService.UserId);
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
