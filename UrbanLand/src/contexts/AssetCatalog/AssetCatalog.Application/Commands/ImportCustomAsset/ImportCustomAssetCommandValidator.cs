using FluentValidation;

namespace AssetCatalog.Application.Commands.ImportCustomAsset;

public class ImportCustomAssetCommandValidator : AbstractValidator<ImportCustomAssetCommand>
{
    public ImportCustomAssetCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.FileUrl).NotEmpty();
        RuleFor(x => x.FileName).NotEmpty();
        RuleFor(x => x.FileSize).InclusiveBetween(1, 100 * 1024 * 1024);
        RuleFor(x => x.Format).NotEmpty();
        RuleFor(x => x.Width).GreaterThan(0);
        RuleFor(x => x.Height).GreaterThan(0);
        RuleFor(x => x.Depth).GreaterThan(0);
        RuleFor(x => x.PolygonCount).GreaterThanOrEqualTo(0);
    }
}
