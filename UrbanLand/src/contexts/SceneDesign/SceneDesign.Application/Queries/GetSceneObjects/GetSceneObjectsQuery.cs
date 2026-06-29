using Core.Primitives;
using MediatR;

namespace SceneDesign.Application.Queries.GetSceneObjects;

public record GetSceneObjectsQuery(Guid SceneId) : IRequest<Result<List<SceneObjectDto>>>;
