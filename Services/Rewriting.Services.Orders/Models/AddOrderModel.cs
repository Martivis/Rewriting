using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.Orders;

public class AddOrderModel
{
    public string Title { get; set; }
    public string Text { get; set; }
    public string Comment { get; set; }
    public Guid ClientUid { get; set; }
}

public class AddOrderModelValidator : AbstractValidator<AddOrderModel>
{
    public AddOrderModelValidator()
    {
        RuleFor(p => p.Title).NotEmpty().WithMessage("Title is required")
            .MaximumLength(50).WithMessage("Title is too long");
        RuleFor(p => p.Text).NotEmpty().WithMessage("Text is required");
        RuleFor(p => p.Comment).NotEmpty().WithMessage("Comment is required");
        RuleFor(p => p.ClientUid).NotEmpty().WithMessage("Guid is required");
    }
}
