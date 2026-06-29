using Core.Primitives;
using MediatR;

namespace SceneDesign.Application.Queries.GetScene;

public record GetSceneQuery(Guid ProjectId) : IRequest<Result<SceneDto>>;
