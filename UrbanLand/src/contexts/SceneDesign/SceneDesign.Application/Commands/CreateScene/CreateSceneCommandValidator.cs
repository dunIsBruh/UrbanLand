using FluentValidation;

namespace SceneDesign.Application.Commands.CreateScene;

public class CreateSceneCommandValidator : AbstractValidator<CreateSceneCommand>
{
    public CreateSceneCommandValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty();
    }
}
