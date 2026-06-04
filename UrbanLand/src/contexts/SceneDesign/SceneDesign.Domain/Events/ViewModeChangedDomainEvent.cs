using SceneDesign.Domain.Enums;
using SceneDesign.Domain.ValueObjects;
using SharedKernel.Abstractions;

namespace SceneDesign.Domain.Events;

public sealed record ViewModeChangedDomainEvent(
    SceneId SceneId,
    ViewMode OldMode,
    ViewMode NewMode) : DomainEvent;
