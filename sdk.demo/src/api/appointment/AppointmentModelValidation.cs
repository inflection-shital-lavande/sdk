using FluentValidation;
using sdk.demo.src.api.appointment.AppointmentModel;

namespace sdk.demo.src.api.appointment.AppointmentValidation;

public class AppointmentCreateModelValidator : AbstractValidator<AppointmentCreateModel>
{
    public AppointmentCreateModelValidator()
    {
        RuleFor(x => x.AssetCode)
            .MaximumLength(100).WithMessage("AssetCode cannot be longer than 100 characters.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name cannot be longer than 200 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot be longer than 1000 characters.");

        RuleFor(x => x.AppointmentType)
            .NotEmpty().WithMessage("AppointmentType is required.");

        RuleFor(x => x.Version)
            .MaximumLength(50).WithMessage("Version cannot be longer than 50 characters.");
    }
}

public class AppointmentUpdateModelValidator : AbstractValidator<AppointmentUpdateModel>
{
    public AppointmentUpdateModelValidator()
    {
        RuleFor(x => x.AssetCode)
            .MaximumLength(100).WithMessage("AssetCode cannot be longer than 100 characters.");

        RuleFor(x => x.Name)
            .MaximumLength(200).WithMessage("Name cannot be longer than 200 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot be longer than 1000 characters.");

        RuleFor(x => x.AppointmentType)
            .NotEmpty().WithMessage("AppointmentType is required.");

        RuleForEach(x => x.Tags)
            .MaximumLength(200).WithMessage("Tags cannot be longer than 200 characters.");

        RuleFor(x => x.Version)
            .MaximumLength(50).WithMessage("Version cannot be longer than 50 characters.");
    }
}

public class AppointmentSearchFiltersValidator : AbstractValidator<AppointmentSearchFilters>
{
    public AppointmentSearchFiltersValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(100).WithMessage("Name cannot be longer than 100 characters.");
    }
}
