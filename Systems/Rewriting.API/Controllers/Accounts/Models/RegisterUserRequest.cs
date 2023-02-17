using AutoMapper;
using FluentValidation;
using Rewriting.Services.UserAccount;

namespace Rewriting.API.Controllers.Accounts;

public class RegisterUserRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class RegisterUserModelRequestValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserModelRequestValidator()
    {
        RuleFor(s => s.FirstName)
            .NotEmpty().WithMessage("First name is required");

        RuleFor(s => s.LastName)
            .NotEmpty().WithMessage("Last name is required");

        RuleFor(s => s.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email");

        RuleFor(s => s.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password should contain at least 6 symbols")
            .MaximumLength(50).WithMessage("Password is too long");
    }
}

public class RegisterUserRequestProfile : Profile
{
    public RegisterUserRequestProfile()
    {
        CreateMap<RegisterUserRequest, RegisterUserModel>();
    }
}