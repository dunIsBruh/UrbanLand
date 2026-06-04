using MediatR;
using SceneDesign.Domain.Enums;
using SharedKernel.Primitives;

namespace SceneDesign.Application.Commands.SwitchViewMode;

public record SwitchViewModeCommand(Guid SceneId, ViewMode ViewMode) : IRequest<Result>;
