using MediatR;
using SceneDesign.Domain.ValueObjects;
using SharedKernel.Primitives;

namespace SceneDesign.Application.Commands.CreateScene;

public record CreateSceneCommand(Guid ProjectId) : IRequest<Result<SceneId>>;
