using System.Xml;
using FluentValidation;
using sdk.demo.src.api.action_plan.ActionPlanModel;

namespace sdk.demo.src.api.action_plan.ActionPlanValidation;
public class ActionPlanCreateModelValidator : AbstractValidator<ActionPlanCreateModel>
{
    public ActionPlanCreateModelValidator()
    {
        RuleFor(x => x.AssetCode)
            .NotEmpty().WithMessage("AssetCode is required.")
            .Length(2, 50).WithMessage("AssetCode must be between 2 and 50 characters.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Length(2, 100).WithMessage("Name must be between 2 and 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.")
            .When(x => x.Description != null);

        RuleFor(x => x.Version)
            .NotEmpty().WithMessage("Version is required.")
            .Length(1, 20).WithMessage("Version must be between 1 and 20 characters.");
    }
}

public class ActionPlanUpdateModelValidator : AbstractValidator<ActionPlanUpdateModel>
{
    public ActionPlanUpdateModelValidator()
    {
        RuleFor(x => x.AssetCode)
            .NotEmpty().WithMessage("AssetCode is required.")
            .Length(2, 50).WithMessage("AssetCode must be between 2 and 50 characters.");

        RuleFor(x => x.Name)
            .Length(2, 100).WithMessage("Name must be between 2 and 100 characters.")
            .When(x => x.Name != null);

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.")
            .When(x => x.Description != null);

        RuleFor(x => x.Tags)
            .Must(tags => tags == null || (tags.Count >= 1 && tags.Count <= 20))
            .WithMessage("The number of tags must be between 1 and 20.");

        RuleFor(x => x.Version)
            .Length(1, 20).WithMessage("Version must be between 1 and 20 characters.")
            .When(x => x.Version != null);
    }
}
public class ActionPlanSearchFiltersValidator : AbstractValidator<ActionPlanSearchFilters>
{
    public ActionPlanSearchFiltersValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Length(2, 100).WithMessage("Name must be between 2 and 100 characters.");
    }
}
public class ApiResponseValidator : AbstractValidator<ApiResponse>
{
    public ApiResponseValidator()
    {
        RuleFor(x => x.Data)
            .NotNull().WithMessage("Data is required.");

        RuleFor(x => x.Message)
            .MaximumLength(500).WithMessage("Message cannot exceed 500 characters.")
            .When(x => x.Message != null);

        RuleFor(x => x.Success)
            .NotNull().WithMessage("Success flag is required.");
    }
}