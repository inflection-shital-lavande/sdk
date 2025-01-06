using FluentValidation;
using sdk.demo.src.api.user.UserModel;

namespace sdk.demo.src.api.user.UserModelValidation;
public class UserCreateModelValidator : AbstractValidator<UserCreateModel>
{
    public UserCreateModelValidator()
    {
        RuleFor(x => x.RoleId).NotEmpty().WithMessage("RoleId is required.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("FirstName is required.")
            .MaximumLength(50).WithMessage("FirstName cannot be longer than 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("LastName is required.")
            .MaximumLength(50).WithMessage("LastName cannot be longer than 50 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid Email Address.")
            .MaximumLength(100).WithMessage("Email cannot be longer than 100 characters.");

        RuleFor(x => x.Gender)
            .NotEmpty().WithMessage("Gender is required.")
            .MaximumLength(10).WithMessage("Gender cannot be longer than 10 characters.");

        RuleFor(x => x.CountryCode)
            .NotEmpty().WithMessage("CountryCode is required.")
            .MaximumLength(5).WithMessage("CountryCode cannot be longer than 5 characters.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone is required.")
            .MaximumLength(15).WithMessage("Phone cannot be longer than 15 characters.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password should be at least 8 characters.")
            .MaximumLength(100).WithMessage("Password cannot be longer than 100 characters.");
    }
}
public class UserUpdateModelValidator : AbstractValidator<UserUpdateModel>
{
    public UserUpdateModelValidator()
    {
      
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("FirstName is required.")
            .MaximumLength(50).WithMessage("FirstName cannot be longer than 50 characters.");

        RuleFor(x => x.LastName)
            .MaximumLength(50).WithMessage("LastName cannot be longer than 50 characters.");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Invalid Email Address.")
            .MaximumLength(100).WithMessage("Email cannot be longer than 100 characters.");

        RuleFor(x => x.Gender)
            .MaximumLength(10).WithMessage("Gender cannot be longer than 10 characters.");

        RuleFor(x => x.CountryCode)
            .MaximumLength(5).WithMessage("CountryCode cannot be longer than 5 characters.");

        RuleFor(x => x.Phone)
            .MaximumLength(15).WithMessage("Phone cannot be longer than 15 characters.");

        RuleFor(x => x.Password)
         .NotEmpty().WithMessage("Password is required.")
         .MinimumLength(8).WithMessage("Password should be at least 8 characters.")
         .MaximumLength(100).WithMessage("Password cannot be longer than 100 characters.");
    }
}
public class UserSearchFiltersValidator : AbstractValidator<UserSearchFilters>
{
    public UserSearchFiltersValidator()
    {
        RuleFor(x => x.FirstName)
            .MaximumLength(50).WithMessage("FirstName cannot be longer than 50 characters.");
    }
}

