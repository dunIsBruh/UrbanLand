using Core.Primitives;
using MediatR;
using SceneDesign.Domain.Repositories;
using ProjectId = Core.Contracts.ProjectId;

namespace SceneDesign.Application.Queries.GetScene;

public class GetSceneQueryHandler(
    ISceneRepository sceneRepository)
    : IRequestHandler<GetSceneQuery, Result<SceneDto>>
{
    public async Task<Result<SceneDto>> Handle(GetSceneQuery query, CancellationToken ct)
    {
        var projectId = ProjectId.From(query.ProjectId);
        var scene = await sceneRepository.GetByProjectIdAsync(projectId, ct);

        if (scene == null)
        {
            return Result<SceneDto>.Failure(Error.NotFound("Scene", $"project {query.ProjectId}"));
        }

        return Result<SceneDto>.Success(SceneDto.FromDomain(scene));
    }
}
