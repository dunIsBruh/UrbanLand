using Core.Abstractions;
using SceneDesign.Domain.Enums;
using SceneDesign.Domain.ValueObjects;

namespace SceneDesign.Domain.Events;

public sealed record ViewModeChangedDomainEvent(
    SceneId SceneId,
    ViewMode OldMode,
    ViewMode NewMode) : DomainEvent;
