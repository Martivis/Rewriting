using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.UserAccount;

public class RegisterUserModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly BirthDate { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class RegisterUserModelValidator : AbstractValidator<RegisterUserModel>
{
    public RegisterUserModelValidator()
    {
        RuleFor(s => s.FirstName)
            .NotEmpty().WithMessage("First name is required");

        RuleFor(s => s.LastName)
            .NotEmpty().WithMessage("Last name is required");

        RuleFor(s => s.BirthDate)
            .Must(s => s < DateOnly.FromDateTime(DateTime.Today)).WithMessage("Invalid birth date");

        RuleFor(s => s.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email");

        RuleFor(s => s.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password should contain at least 6 symbols")
            .MaximumLength(50).WithMessage("Password is too long");
    }
}