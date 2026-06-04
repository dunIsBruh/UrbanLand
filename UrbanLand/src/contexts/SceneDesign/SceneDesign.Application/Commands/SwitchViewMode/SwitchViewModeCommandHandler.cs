using MediatR;
using SceneDesign.Domain.Entities;
using SceneDesign.Domain.Repositories;
using SceneDesign.Domain.ValueObjects;
using SharedKernel.Primitives;

namespace SceneDesign.Application.Commands.SwitchViewMode;

public class SwitchViewModeCommandHandler(
    ISceneRepository sceneRepository)
    : IRequestHandler<SwitchViewModeCommand, Result>
{
    public async Task<Result> Handle(SwitchViewModeCommand command, CancellationToken ct)
    {
        var sceneId = SceneId.From(command.SceneId);
        var scene = await sceneRepository.GetByIdAsync(sceneId, ct);
        if (scene == null)
            return Result.Failure(Error.NotFound(nameof(Scene), sceneId));

        var result = scene.SwitchViewMode(command.ViewMode);
        if (result.IsFailure)
            return result;

        await sceneRepository.SaveAsync(scene, ct);
        return Result.Success();
    }
}
