using AutoMapper;
using FluentValidation;
using Rewriting.Services.UserAccount;

namespace Rewriting.API.Controllers.Accounts;

public class ResetPasswordRequest
{
    public string Token { get; set; }
    public string Email { get; set; }
    public string NewPassword { get; set; }
}

public class ResetPasswordValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordValidator()
    {
        RuleFor(p => p.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email");

        RuleFor(p => p.Token)
            .NotEmpty().WithMessage("Token is required");

        RuleFor(p => p.NewPassword)
            .NotEmpty().WithMessage("New password is required")
            .MinimumLength(6).WithMessage("Password should contain at least 6 symbols")
            .MaximumLength(50).WithMessage("Password is too long");
    }
}
public class ResetPasswordRequestProfile : Profile
{
    public ResetPasswordRequestProfile()
    {
        CreateMap<ResetPasswordRequest, ResetPasswordModel>();
    }
}