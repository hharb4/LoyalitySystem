using FluentValidation;
using LoyalitySystem.Contracts.Requests;

namespace LoyalitySystem.Host.Validators
{
    public class EarnPointsRequestValidator : AbstractValidator<EarnPointsInputModel>
    {
        public EarnPointsRequestValidator()
        {
            RuleFor(x => x.Points)
                .GreaterThan(0).WithMessage("Points must be greater than zero.");
        }
    }
}
