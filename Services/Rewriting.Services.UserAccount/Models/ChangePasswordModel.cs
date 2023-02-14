using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.UserAccount;

public class ChangePasswordModel
{
    public ClaimsPrincipal Issuer { get; set; }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}

public class ChangePasswordModelValidator : AbstractValidator<ChangePasswordModel>
{
    public ChangePasswordModelValidator()
    {
        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("New password is required")
            .MinimumLength(6).WithMessage("Password should contain at least 6 symbols")
            .MaximumLength(50).WithMessage("Password is too long");

        RuleFor(x => x.OldPassword)
            .NotEmpty().WithMessage("Old password is required")
            .MinimumLength(6).WithMessage("Password should contain at least 6 symbols")
            .MaximumLength(50).WithMessage("Password is too long");
    }
}
