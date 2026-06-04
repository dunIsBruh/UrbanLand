using SceneDesign.Domain.ValueObjects;
using SharedKernel.Abstractions;
using ProjectId = SharedKernel.Contracts.ProjectId;

namespace SceneDesign.Domain.Events;

public sealed record SceneCreatedDomainEvent(SceneId SceneId, ProjectId ProjectId) : DomainEvent;