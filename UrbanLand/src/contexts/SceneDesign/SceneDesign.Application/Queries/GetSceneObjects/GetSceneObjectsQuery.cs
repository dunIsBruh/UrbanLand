using MediatR;
using SharedKernel.Primitives;

namespace SceneDesign.Application.Queries.GetSceneObjects;

public record GetSceneObjectsQuery(Guid SceneId) : IRequest<Result<List<SceneObjectDto>>>;
