using AutoMapper;
using FluentValidation;
using Rewriting.Services.UserAccount;

namespace Rewriting.API.Controllers.Accounts;

public class ChangePasswordRequest
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordModel>
{
    public ChangePasswordRequestValidator()
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

public class ChangePasswordRequestProfile : Profile
{
    public ChangePasswordRequestProfile()
    {
        CreateMap<ChangePasswordRequest, ChangePasswordModel>()
            .ForMember(s => s.Issuer, o => o.Ignore());
    }
}
