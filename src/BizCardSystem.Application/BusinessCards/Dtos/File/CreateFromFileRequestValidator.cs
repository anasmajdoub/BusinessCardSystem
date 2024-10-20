using BizCardSystem.Application.Shared.Validators;
using FluentValidation;

namespace BizCardSystem.Application.BusinessCards.Dtos.File
{
    public class CreateFromFileRequestValidator : AbstractValidator<CreateFromFileRequest>
    {
        public CreateFromFileRequestValidator()
        {
            RuleFor(x => x.File)
            .NotEmpty().WithMessage("File is required.")
            .Must(CustomValidator.BeValidFileType).WithMessage("Only XML, CSV, or QR code files are allowed.");
        }
    }
}
