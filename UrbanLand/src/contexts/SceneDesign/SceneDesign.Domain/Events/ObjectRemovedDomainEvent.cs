using Core.Abstractions;
using SceneDesign.Domain.ValueObjects;

namespace SceneDesign.Domain.Events;

public sealed record ObjectRemovedDomainEvent(
    SceneId SceneId,
    SceneObjectId ObjectId,
    AssetId AssetId) : DomainEvent;