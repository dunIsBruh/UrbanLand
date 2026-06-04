using SceneDesign.Domain.ValueObjects;
using SharedKernel.Abstractions;

namespace SceneDesign.Domain.Events;

public sealed record ObjectMovedDomainEvent(
    SceneId SceneId,
    SceneObjectId ObjectId,
    Position3D OldPosition,
    Position3D NewPosition) : DomainEvent;