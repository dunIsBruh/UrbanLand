using MediatR;
using SharedKernel.Primitives;

namespace SceneDesign.Application.Queries.GetScene;

public record GetSceneQuery(Guid ProjectId) : IRequest<Result<SceneDto>>;
