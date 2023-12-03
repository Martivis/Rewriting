using AutoMapper;
using FluentValidation;
using Rewriting.Services.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.API.Controllers.Orders;

public class AddOrderRequest
{
    public string Title { get; set; }
    public string Text { get; set; }
    public string Comment { get; set; }
}

public class AddOrderRequestValidator : AbstractValidator<AddOrderRequest>
{
    public AddOrderRequestValidator()
    {
        RuleFor(p => p.Title).NotEmpty().WithMessage("Title is required")
            .MaximumLength(50).WithMessage("Title is too long");
        RuleFor(p => p.Text).MinimumLength(20).WithMessage("Text is required");
        RuleFor(p => p.Comment).NotEmpty().WithMessage("Comment is required");
    }
}

public class AddOrderRequestProfile : Profile
{
    public AddOrderRequestProfile()
    {
        CreateMap<AddOrderRequest, AddOrderModel>();
    }
}
