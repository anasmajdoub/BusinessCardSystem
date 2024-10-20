using BizCardSystem.Domain.Shared;
using FluentValidation;

namespace BizCardSystem.Application.Shared.Validators;

public class AddressValidator : AbstractValidator<Address>
{
    public AddressValidator()
    {
        RuleFor(a => a.Street)
            .MaximumLength(100).WithMessage("Street cannot exceed 100 characters.")
            .When(a => !string.IsNullOrEmpty(a.Street));

        RuleFor(a => a.City)
            .MaximumLength(50).WithMessage("City cannot exceed 50 characters.")
            .When(a => !string.IsNullOrEmpty(a.City));

        RuleFor(a => a.State)
            .MaximumLength(50).WithMessage("State cannot exceed 50 characters.")
            .When(a => !string.IsNullOrEmpty(a.City));


        RuleFor(a => a.Country)
            .MaximumLength(50).WithMessage("Country cannot exceed 50 characters.")
            .When(a => !string.IsNullOrEmpty(a.Country));
    }
}
