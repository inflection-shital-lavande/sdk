using FluentValidation;
using sdk.demo.src.api.animation.AnimationModel;

namespace sdk.demo.src.api.animation.AnimationValidation;

public class AnimationCreateModelValidator : AbstractValidator<AnimationCreateModel>
{
    public AnimationCreateModelValidator()
    {
        RuleFor(x => x.AssetCode)
            .MaximumLength(100).WithMessage("AssetCode cannot be longer than 100 characters.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name cannot be longer than 200 characters.");

        RuleFor(x => x.Transcript)
            .MaximumLength(1000).WithMessage("Transcript cannot be longer than 1000 characters.");

        RuleFor(x => x.Url)
            .MaximumLength(500).WithMessage("Url cannot be longer than 500 characters.");

        RuleFor(x => x.Version)
            .MaximumLength(50).WithMessage("Version cannot be longer than 50 characters.");
    }
}

public class AnimationUpdateModelValidator : AbstractValidator<AnimationUpdateModel>
{
    public AnimationUpdateModelValidator()
    {
        RuleFor(x => x.AssetCode)
            .MaximumLength(100).WithMessage("AssetCode cannot be longer than 100 characters.");

        RuleFor(x => x.Name)
            .MaximumLength(200).WithMessage("Name cannot be longer than 200 characters.");

        RuleFor(x => x.Transcript)
            .MaximumLength(1000).WithMessage("Transcript cannot be longer than 1000 characters.");

        RuleFor(x => x.Url)
            .MaximumLength(500).WithMessage("Url cannot be longer than 500 characters.");

        RuleFor(x => x.Tags)
            .Must(tags => tags == null || tags.All(tag => tag.Length <= 200))
            .WithMessage("Each tag cannot be longer than 200 characters.");

        RuleFor(x => x.Version)
            .MaximumLength(50).WithMessage("Version cannot be longer than 50 characters.");
    }
}

public class AnimationSearchFiltersValidator : AbstractValidator<AnimationSearchFilters>
{
    public AnimationSearchFiltersValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(200).WithMessage("Name cannot be longer than 200 characters.");
    }
}

