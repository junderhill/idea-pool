using FluentValidation;
using MyIdeaPool.ViewModels;

namespace MyIdeaPool.Validators
{
    public class UserSignupViewModelValidator : AbstractValidator<UserSignupViewModel>
    {
        public UserSignupViewModelValidator()
        {
            RuleFor(x => x.password).MinimumLength(8).WithMessage("Password must be at least 8 characters in length.");
            RuleFor(x => x.password).Matches("[A-Z]").WithMessage("Password must contain at least 1 uppercase character.");
            RuleFor(x => x.password).Matches("[a-z]").WithMessage("Password must contain as least 1 lowercase character.");
            RuleFor(x => x.password).Matches("\\d").WithMessage("Password must contain at least 1 number.");

            RuleFor(x => x.name).NotEmpty().WithMessage("Name must not be empty.");

            RuleFor(x => x.email).NotEmpty().WithMessage("Email address must be specified.")
                .EmailAddress().WithMessage("A valid email address must be specified.");
        }
    }
}