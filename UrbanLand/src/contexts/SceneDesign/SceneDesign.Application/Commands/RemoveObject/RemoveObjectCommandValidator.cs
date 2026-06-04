using FluentValidation;

namespace SceneDesign.Application.Commands.RemoveObject;

public class RemoveObjectCommandValidator : AbstractValidator<RemoveObjectCommand>
{
    public RemoveObjectCommandValidator()
    {
        RuleFor(x => x.SceneId).NotEmpty();
        RuleFor(x => x.ObjectId).NotEmpty();
    }
}
