using FluentValidation;

namespace Rewriting.API.Controllers.Contracts;

public class AddResultRequestValidator : AbstractValidator<AddResultRequest>
{
    public AddResultRequestValidator()
    {
        RuleFor(p => p.Text).NotEmpty().WithMessage("Text is required");
    }
}
