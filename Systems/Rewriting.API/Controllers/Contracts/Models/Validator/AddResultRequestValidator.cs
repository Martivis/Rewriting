using FluentValidation;

namespace Rewriting.API.Controllers.Contracts;

public class AddResultRequestValidator : AbstractValidator<AddResultRequest>
{
    public AddResultRequestValidator()
    {
        RuleFor(p => p.Text).MinimumLength(20).WithMessage("Text is required");
    }
}
