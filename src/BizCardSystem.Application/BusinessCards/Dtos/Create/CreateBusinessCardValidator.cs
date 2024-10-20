using BizCardSystem.Application.Shared.Validators;
using BizCardSystem.Domain.BusinessCards;
using FluentValidation;

namespace BizCardSystem.Application.BusinessCards.Dtos.Create
{
    public class CreateBusinessCardValidator : AbstractValidator<CreateBizRequest>
    {
        public CreateBusinessCardValidator(IBusinessCardsRepository businessCardsRepository)
        {
            RuleFor(card => card.Name)
               .NotEmpty().WithMessage("Name is required.")
               .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(card => card.Gender)
            .IsInEnum().WithMessage("Invalid gender selection.");

            RuleFor(card => card.DateofBirth)
            .NotEmpty().WithMessage("Date of birth is required.")
            .LessThan(DateTime.Now).WithMessage("Date of birth cannot be in the future.")
            .GreaterThan(DateTime.Now.AddYears(-120)).WithMessage("Age cannot exceed 120 years.");

            RuleFor(card => card.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");



            RuleFor(card => card.Photo)
                .NotEmpty().WithMessage("Photo is required.")
                .Must(CustomValidator.BeValidBase64).WithMessage("The provided string is not a valid base64 string.")
                .Must(CustomValidator.BeLessThan1MB).WithMessage("Image file size must be less than 1 MB.");


            RuleFor(card => card.Address)
               .NotNull().WithMessage("Address is required.")
               .SetValidator(new AddressValidator());
        }
    }
}
