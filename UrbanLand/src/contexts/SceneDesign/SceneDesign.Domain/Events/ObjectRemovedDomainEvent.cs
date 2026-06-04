using SceneDesign.Domain.ValueObjects;
using SharedKernel.Abstractions;

namespace SceneDesign.Domain.Events;

public sealed record ObjectRemovedDomainEvent(
    SceneId SceneId,
    SceneObjectId ObjectId,
    AssetId AssetId) : DomainEvent;