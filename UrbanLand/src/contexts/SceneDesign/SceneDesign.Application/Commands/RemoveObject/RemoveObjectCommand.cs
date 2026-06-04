using MediatR;
using SharedKernel.Primitives;

namespace SceneDesign.Application.Commands.RemoveObject;

public record RemoveObjectCommand(Guid SceneId, Guid ObjectId) : IRequest<Result>;
