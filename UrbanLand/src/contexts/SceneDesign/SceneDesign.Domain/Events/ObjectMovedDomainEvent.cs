using Core.Abstractions;
using SceneDesign.Domain.ValueObjects;

namespace SceneDesign.Domain.Events;

public sealed record ObjectMovedDomainEvent(
    SceneId SceneId,
    SceneObjectId ObjectId,
    Position3D OldPosition,
    Position3D NewPosition) : DomainEvent;