using Core.Primitives;
using MediatR;

namespace SceneDesign.Application.Commands.RemoveObject;

public record RemoveObjectCommand(Guid SceneId, Guid ObjectId) : IRequest<Result>;
