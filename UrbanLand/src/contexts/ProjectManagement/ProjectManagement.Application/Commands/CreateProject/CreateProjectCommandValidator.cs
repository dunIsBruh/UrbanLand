using FluentValidation;

namespace ProjectManagement.Application.Commands.CreateProject;

public class CreateProjectValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Project name is required")
            .MinimumLength(3).WithMessage("Project name must be at least 3 characters")
            .MaximumLength(200).WithMessage("Project name must be less than 200 characters")
            .Matches(@"^[a-zA-Z0-9\s\-_]+$").WithMessage("Project name contains invalid characters");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description must be less than 2000 characters");

        // RuleFor(x => x.SceneType)
        //     .NotEmpty().WithMessage("Scene type is required")
        //     .Must(BeValidProjectType).WithMessage("Invalid scene type");
    }

    // private static bool BeValidProjectType(string type)
    // {
    //     return Enum.TryParse<SceneType>(type, ignoreCase: true, out _);
    // }
}