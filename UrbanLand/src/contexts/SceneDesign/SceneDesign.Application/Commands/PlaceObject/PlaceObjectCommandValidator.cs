using FluentValidation;

namespace SceneDesign.Application.Commands.PlaceObject;

public class PlaceObjectCommandValidator : AbstractValidator<PlaceObjectCommand>
{
    public PlaceObjectCommandValidator()
    {
        RuleFor(x => x.SceneId).NotEmpty();
        RuleFor(x => x.AssetId).NotEmpty();
    }
}
