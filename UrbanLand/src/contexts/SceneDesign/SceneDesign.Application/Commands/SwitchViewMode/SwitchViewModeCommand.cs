using Core.Primitives;
using MediatR;
using SceneDesign.Domain.Enums;

namespace SceneDesign.Application.Commands.SwitchViewMode;

public record SwitchViewModeCommand(Guid SceneId, ViewMode ViewMode) : IRequest<Result>;
