using FluentValidation;
using MyIdeaPool.Models;

namespace MyIdeaPool.Validators
{
    public class IdeaViewModelValidator : AbstractValidator<IdeaViewModel>
    {
        public IdeaViewModelValidator()
        {
            RuleFor(x => x.content)
                .NotEmpty()
                .WithMessage("Idea content is required")
                .MaximumLength(255)
                .WithMessage("Idea content must be 255 characters or less");

            RuleFor(x => x.impact)
                .GreaterThan(0)
                .LessThanOrEqualTo(10)
                .WithMessage("Impact must be between 1-10");
            RuleFor(x => x.ease)
                .GreaterThan(0)
                .LessThanOrEqualTo(10)
                .WithMessage("Ease must be between 1-10");
            RuleFor(x => x.confidence)
                .GreaterThan(0)
                .LessThanOrEqualTo(10)
                .WithMessage("Confidence must be between 1-10");
        } 
    }
}