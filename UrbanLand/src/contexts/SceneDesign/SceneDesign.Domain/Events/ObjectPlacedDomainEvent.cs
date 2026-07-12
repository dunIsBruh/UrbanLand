using Core.Abstractions;
using SceneDesign.Domain.ValueObjects;

namespace SceneDesign.Domain.Events;

public sealed record ObjectPlacedDomainEvent(
    SceneId SceneId,
    SceneObjectId ObjectId,
    AssetId AssetId,
    Position3D Position) : DomainEvent;