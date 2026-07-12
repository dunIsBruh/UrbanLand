using Core.Primitives;
using MediatR;
using SceneDesign.Domain.ValueObjects;

namespace SceneDesign.Application.Commands.CreateScene;

public record CreateSceneCommand(Guid ProjectId) : IRequest<Result<SceneId>>;
