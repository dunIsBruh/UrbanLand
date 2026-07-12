using Core.Abstractions;
using Core.Contracts;
using SceneDesign.Domain.ValueObjects;

namespace SceneDesign.Domain.Events;

public sealed record SceneCreatedDomainEvent(SceneId SceneId, ProjectId ProjectId) : DomainEvent;