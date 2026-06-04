using FluentValidation;

namespace SceneDesign.Application.Commands.SwitchViewMode;

public class SwitchViewModeCommandValidator : AbstractValidator<SwitchViewModeCommand>
{
    public SwitchViewModeCommandValidator()
    {
        RuleFor(x => x.SceneId).NotEmpty();
        RuleFor(x => x.ViewMode).IsInEnum();
    }
}
