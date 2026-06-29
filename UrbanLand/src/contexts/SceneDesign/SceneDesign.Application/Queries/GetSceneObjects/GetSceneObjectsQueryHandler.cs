using Core.Primitives;
using MediatR;
using SceneDesign.Domain.Repositories;
using SceneDesign.Domain.ValueObjects;
using SceneDesign.Domain.Entities;

namespace SceneDesign.Application.Queries.GetSceneObjects;

public class GetSceneObjectsQueryHandler(
    ISceneRepository sceneRepository)
    : IRequestHandler<GetSceneObjectsQuery, Result<List<SceneObjectDto>>>
{
    public async Task<Result<List<SceneObjectDto>>> Handle(GetSceneObjectsQuery query, CancellationToken ct)
    {
        var sceneId = SceneId.From(query.SceneId);
        var scene = await sceneRepository.GetByIdAsync(sceneId, ct);

        if (scene == null)
        {
            return Result<List<SceneObjectDto>>.Failure(Error.NotFound(nameof(Scene), sceneId));
        }

        var dtos = scene.Objects.Select(SceneObjectDto.FromDomain).ToList();
        return Result<List<SceneObjectDto>>.Success(dtos);
    }
}
