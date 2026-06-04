using FluentValidation;

namespace ProjectManagement.Application.Commands.AddMember;

public class AddObserverCommandValidator : AbstractValidator<AddMemberCommand>
{
    public AddObserverCommandValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty().WithMessage("Project ID is required");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("Observer ID is required");
    }
}