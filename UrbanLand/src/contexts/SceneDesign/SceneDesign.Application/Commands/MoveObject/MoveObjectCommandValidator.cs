using FluentValidation;

namespace SceneDesign.Application.Commands.MoveObject;

public class MoveObjectCommandValidator : AbstractValidator<MoveObjectCommand>
{
    public MoveObjectCommandValidator()
    {
        RuleFor(x => x.SceneId).NotEmpty();
        RuleFor(x => x.ObjectId).NotEmpty();
    }
}
