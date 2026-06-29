using Core.Contracts;
using Core.Primitives;
using MediatR;
using SceneDesign.Domain.Entities;
using SceneDesign.Domain.Repositories;
using SceneDesign.Domain.ValueObjects;

namespace SceneDesign.Application.Commands.CreateScene;

public class CreateSceneCommandHandler(
    ISceneRepository sceneRepository)
    : IRequestHandler<CreateSceneCommand, Result<SceneId>>
{
    public async Task<Result<SceneId>> Handle(CreateSceneCommand command, CancellationToken ct)
    {
        var projectId = ProjectId.From(command.ProjectId);

        var existingScene = await sceneRepository.GetByProjectIdAsync(projectId, ct);
        if (existingScene != null)
        {
            return Result<SceneId>.Failure(Error.Conflict("Scene already exists for this project"));
        }

        var result = Scene.CreateForProject(projectId);
        if (result.IsFailure)
        {
            return Result<SceneId>.Failure(result.Error);
        }

        var scene = result.Value;
        await sceneRepository.SaveAsync(scene, ct);

        return Result<SceneId>.Success(scene.Id);
    }
}
